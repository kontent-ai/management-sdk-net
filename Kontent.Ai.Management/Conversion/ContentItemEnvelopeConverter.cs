using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;
using Kontent.Ai.Management.Serialization.Converters;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Kontent.Ai.Management.Conversion;

/// <summary>
/// Translates between generated content-type records (implementing <see cref="IContentItem"/>) and the MAPI
/// language-variant <c>elements</c> wire shape — an array of envelopes of the form
/// <c>{ "element": { "codename" | "id": "..." }, "value": &lt;kind-specific&gt;, "components": [...] }</c>.
/// </summary>
/// <remarks>
/// <para>
/// <b>Write direction</b> emits envelopes keyed by element <em>codename</em> (what MAPI accepts on POST/PUT).
/// Properties whose value is <c>null</c> are skipped — partial-update semantics, matching the migration / scripted-write
/// usage we expect to be the dominant consumer.
/// </para>
/// <para>
/// <b>Read direction</b> matches envelopes by element <em>id</em> first (what MAPI returns), falling back to codename
/// if the response is codename-keyed. Unknown elements in the response are skipped — forward-compat with future MAPI changes.
/// </para>
/// <para>
/// Thread-safe; intended to be shared across calls. Reflection cost is paid once per content type
/// (cached in <see cref="ContentItemTypeDescriptor"/>); each (de)serialization allocates only its output buffer plus a
/// list per collection-valued property.
/// </para>
/// </remarks>
internal sealed class ContentItemEnvelopeConverter
{
    private readonly ContentTypeRegistry _registry;
    private readonly JsonSerializerOptions _scalarOptions;

    public ContentItemEnvelopeConverter(ContentTypeRegistry? registry = null, JsonSerializerOptions? scalarOptions = null)
    {
        _registry = registry ?? new ContentTypeRegistry();
        _scalarOptions = scalarOptions ?? CreateDefaultScalarOptions();
    }

    /// <summary>The codename↔type registry this converter dispatches rich-text components through.</summary>
    public ContentTypeRegistry Registry => _registry;

    // ---- Primary API ----

    /// <summary>Writes <paramref name="item"/>'s <c>[KontentElement]</c> properties as a JSON array of envelopes onto <paramref name="writer"/>.</summary>
    public void WriteEnvelopes(Utf8JsonWriter writer, IContentItem item)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(item);

        var descriptor = ContentItemTypeDescriptor.For(item.GetType());

        writer.WriteStartArray();
        foreach (var prop in descriptor.Properties)
        {
            var value = prop.Property.GetValue(item);
            if (value is null) continue;

            writer.WriteStartObject();
            writer.WritePropertyName("element");
            writer.WriteStartObject();
            writer.WriteString("codename", prop.ElementCodename);
            writer.WriteEndObject();
            WriteValue(writer, prop, value);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }

    /// <summary>Reads a JSON envelopes array into a new instance of <paramref name="contentType"/>.</summary>
    public IContentItem ReadEnvelopes(JsonElement envelopesArray, Type contentType)
    {
        if (envelopesArray.ValueKind != JsonValueKind.Array)
        {
            throw new ArgumentException($"Expected a JSON array, got {envelopesArray.ValueKind}.", nameof(envelopesArray));
        }
        return ReadEnvelopes(envelopesArray.EnumerateArray(), contentType);
    }

    /// <summary>Reads element envelopes (typically <c>LanguageVariantModel.Elements</c>) into a new instance of <paramref name="contentType"/>.</summary>
    public IContentItem ReadEnvelopes(IEnumerable<JsonElement> envelopes, Type contentType)
    {
        ArgumentNullException.ThrowIfNull(envelopes);
        ArgumentNullException.ThrowIfNull(contentType);

        // Auto-register the root type itself so its codename is resolvable on recursive descent (rich-text components).
        // Other candidate types (those that may appear as embedded components) must be pre-registered by the caller
        // via ContentTypeRegistry — auto-scanning the whole assembly would risk codename collisions in projects that
        // co-locate multiple content-model sets.
        _registry.Register(contentType);

        var descriptor = ContentItemTypeDescriptor.For(contentType);
        var instance = (IContentItem)Activator.CreateInstance(contentType)!;

        foreach (var envelope in envelopes)
        {
            if (!envelope.TryGetProperty("element", out var elementMeta)) continue;

            var prop = ResolveProperty(descriptor, elementMeta);
            if (prop is null) continue;

            if (!envelope.TryGetProperty("value", out var valueElement)) continue;

            var value = ReadValue(valueElement, prop, envelope);
            prop.Property.SetValue(instance, value);
        }

        return instance;
    }

    // ---- Test / typed convenience ----

    public string WriteEnvelopes<T>(T item) where T : IContentItem
    {
        ArgumentNullException.ThrowIfNull(item);
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            WriteEnvelopes(writer, item);
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public T ReadEnvelopes<T>(IEnumerable<JsonElement> envelopes) where T : IContentItem
        => (T)ReadEnvelopes(envelopes, typeof(T));

    public T ReadEnvelopes<T>(string envelopesJson) where T : IContentItem
    {
        using var doc = JsonDocument.Parse(envelopesJson);
        return (T)ReadEnvelopes(doc.RootElement, typeof(T));
    }

    // ---- Value writers ----

    private void WriteValue(Utf8JsonWriter writer, ContentItemPropertyDescriptor prop, object value)
    {
        switch (prop.Kind)
        {
            case ElementKind.Text:
                writer.WriteString("value", (string)value);
                break;
            case ElementKind.Number:
                writer.WritePropertyName("value");
                JsonSerializer.Serialize(writer, (decimal)value, _scalarOptions);
                break;
            case ElementKind.DateTime:
                var dateTime = (DateTimeValue)value;
                writer.WritePropertyName("value");
                // The API stores a UTC instant (".../Z") with the zone carried separately in display_timezone.
                // UtcDateTime is Kind=Utc, so STJ emits the "Z" form the API returns rather than a numeric offset.
                JsonSerializer.Serialize(writer, dateTime.Value.UtcDateTime, _scalarOptions);
                if (dateTime.DisplayTimeZone is not null) writer.WriteString("display_timezone", dateTime.DisplayTimeZone);
                break;
            case ElementKind.UrlSlug:
                var urlSlug = (UrlSlugValue)value;
                writer.WriteString("value", urlSlug.Value);
                writer.WritePropertyName("mode");
                JsonSerializer.Serialize(writer, urlSlug.Mode, _scalarOptions);
                break;
            case ElementKind.Custom:
                var custom = (CustomValue)value;
                writer.WriteString("value", custom.Value);
                if (custom.SearchableValue is not null) writer.WriteString("searchable_value", custom.SearchableValue);
                break;
            case ElementKind.MultipleChoice:
                WriteMultipleChoice(writer, prop, (IEnumerable)value);
                break;
            case ElementKind.Asset:
                writer.WritePropertyName("value");
                JsonSerializer.Serialize(writer, (IReadOnlyList<AssetReference>)value, _scalarOptions);
                break;
            case ElementKind.Reference:
                writer.WritePropertyName("value");
                JsonSerializer.Serialize(writer, (IReadOnlyList<Reference>)value, _scalarOptions);
                break;
            case ElementKind.RichText:
                WriteRichText(writer, (RichTextElement)value);
                break;
            default:
                throw new NotSupportedException($"Unknown element kind: {prop.Kind}");
        }
    }

    private static void WriteMultipleChoice(Utf8JsonWriter writer, ContentItemPropertyDescriptor prop, IEnumerable values)
    {
        var enumDescriptor = EnumDescriptor.For(prop.CollectionElementType!);
        writer.WritePropertyName("value");
        writer.WriteStartArray();
        foreach (var value in values)
        {
            if (!enumDescriptor.CodenameByValue.TryGetValue(value, out var codename))
            {
                throw new InvalidOperationException(
                    $"Enum value '{value}' on '{prop.Property.DeclaringType?.Name}.{prop.Property.Name}' has no [KontentEnumValue] mapping.");
            }
            writer.WriteStartObject();
            writer.WriteString("codename", codename);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }

    private void WriteRichText(Utf8JsonWriter writer, RichTextElement value)
    {
        writer.WriteString("value", value.Value);
        if (value.Components is { Count: > 0 } components)
        {
            writer.WritePropertyName("components");
            writer.WriteStartArray();
            foreach (var component in components)
            {
                WriteComponent(writer, component);
            }
            writer.WriteEndArray();
        }
    }

    private void WriteComponent(Utf8JsonWriter writer, Component component)
    {
        var contentTypeAttr = component.Content.GetType().GetCustomAttribute<KontentTypeAttribute>()
            ?? throw new InvalidOperationException(
                $"Component.Content type '{component.Content.GetType().FullName}' lacks [KontentType]; cannot serialize as a rich-text component.");

        writer.WriteStartObject();
        writer.WriteString("id", component.Id);
        writer.WritePropertyName("type");
        writer.WriteStartObject();
        writer.WriteString("codename", contentTypeAttr.Codename);
        writer.WriteEndObject();
        writer.WritePropertyName("elements");
        WriteEnvelopes(writer, component.Content);
        writer.WriteEndObject();
    }

    // ---- Value readers ----

    private object? ReadValue(JsonElement value, ContentItemPropertyDescriptor prop, JsonElement envelope)
    {
        if (value.ValueKind == JsonValueKind.Null) return null;

        return prop.Kind switch
        {
            ElementKind.Text => value.GetString(),
            ElementKind.Number => value.GetDecimal(),
            ElementKind.DateTime => new DateTimeValue
            {
                Value = value.GetDateTimeOffset(),
                DisplayTimeZone = envelope.TryGetProperty("display_timezone", out var tz) && tz.ValueKind == JsonValueKind.String
                    ? tz.GetString()
                    : null,
            },
            ElementKind.UrlSlug => new UrlSlugValue
            {
                Value = value.GetString(),
                Mode = envelope.TryGetProperty("mode", out var mode) && mode.ValueKind == JsonValueKind.String
                    ? JsonSerializer.Deserialize<UrlSlugMode>(mode.GetRawText(), _scalarOptions)
                    : default,
            },
            ElementKind.Custom => new CustomValue
            {
                Value = value.GetString(),
                SearchableValue = envelope.TryGetProperty("searchable_value", out var searchable) && searchable.ValueKind == JsonValueKind.String
                    ? searchable.GetString()
                    : null,
            },
            ElementKind.MultipleChoice => ReadMultipleChoice(value, prop),
            ElementKind.Asset => JsonSerializer.Deserialize(value.GetRawText(), prop.Property.PropertyType, _scalarOptions),
            ElementKind.Reference => JsonSerializer.Deserialize(value.GetRawText(), prop.Property.PropertyType, _scalarOptions),
            ElementKind.RichText => ReadRichText(value, envelope),
            _ => throw new NotSupportedException($"Unknown element kind: {prop.Kind}"),
        };
    }

    private static object ReadMultipleChoice(JsonElement value, ContentItemPropertyDescriptor prop)
    {
        var enumType = prop.CollectionElementType!;
        var enumDescriptor = EnumDescriptor.For(enumType);
        var listType = typeof(List<>).MakeGenericType(enumType);
        var list = (IList)Activator.CreateInstance(listType)!;

        foreach (var entry in value.EnumerateArray())
        {
            object? member = null;
            if (entry.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.String)
            {
                enumDescriptor.ByItemId.TryGetValue(idProp.GetString()!, out member);
            }
            if (member is null && entry.TryGetProperty("codename", out var codeProp) && codeProp.ValueKind == JsonValueKind.String)
            {
                enumDescriptor.ByCodename.TryGetValue(codeProp.GetString()!, out member);
            }
            if (member is not null) list.Add(member);
        }
        return list;
    }

    private RichTextElement ReadRichText(JsonElement valueElement, JsonElement envelope)
    {
        var html = valueElement.GetString()!;

        List<Component>? components = null;
        if (envelope.TryGetProperty("components", out var componentsArray)
            && componentsArray.ValueKind == JsonValueKind.Array
            && componentsArray.GetArrayLength() > 0)
        {
            components = new List<Component>(componentsArray.GetArrayLength());
            foreach (var componentElem in componentsArray.EnumerateArray())
            {
                components.Add(ReadComponent(componentElem));
            }
        }

        return new RichTextElement { Value = html, Components = components };
    }

    private Component ReadComponent(JsonElement componentElem)
    {
        var id = componentElem.GetProperty("id").GetGuid();
        var typeCodename = componentElem.GetProperty("type").GetProperty("codename").GetString()!;

        var contentType = _registry.Resolve(typeCodename)
            ?? throw new InvalidOperationException(
                $"Component references content type codename '{typeCodename}', which is not registered. Scan the type's assembly via ContentTypeRegistry before reading.");

        var elements = componentElem.GetProperty("elements");
        var content = ReadEnvelopes(elements, contentType);

        return new Component { Id = id, Content = content };
    }

    // ---- Internals ----

    private static ContentItemPropertyDescriptor? ResolveProperty(ContentItemTypeDescriptor descriptor, JsonElement elementMeta)
    {
        if (elementMeta.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.String
            && descriptor.ByElementId.TryGetValue(idProp.GetString()!, out var byId))
        {
            return byId;
        }
        if (elementMeta.TryGetProperty("codename", out var codeProp) && codeProp.ValueKind == JsonValueKind.String
            && descriptor.ByElementCodename.TryGetValue(codeProp.GetString()!, out var byCodename))
        {
            return byCodename;
        }
        return null;
    }

    private static JsonSerializerOptions CreateDefaultScalarOptions() => new(JsonSerializerDefaults.Web)
    {
        // AssetReference carries its identifier fields as nullable — WhenWritingNull keeps only the populated one(s)
        // on the wire, honouring the MAPI's "any one of id / codename / external_id" contract. Reference is factory-
        // constructed, so it (de)serializes only through ReferenceJsonConverter, which also enforces that contract.
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ReferenceJsonConverter(), new EnumMemberJsonConverterFactory() },
    };
}
