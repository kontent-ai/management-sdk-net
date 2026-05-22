using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Serializes <see cref="IListingResponse{T}"/> server models as a JSON object with the
/// domain-named items collection (e.g. <c>"languages"</c>, <c>"collections"</c>) + <c>"pagination"</c>.
/// </summary>
/// <remarks>
/// Newtonsoft used <c>[JsonObject]</c> to force the object shape on these
/// <see cref="System.Collections.Generic.IEnumerable{T}"/>-implementing types; System.Text.Json has
/// no equivalent and would otherwise serialize them as bare arrays, breaking every paginated list
/// response. The factory matches the abstract base shape and the converter writes/reads the two
/// typed properties directly — no <see cref="JsonSerializer.Serialize"/> call recurses into this
/// converter because neither the items collection nor <see cref="PaginationResponseModel"/> match
/// <see cref="CanConvert"/>.
/// </remarks>
internal sealed class ListingResponseJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert is { IsClass: true, IsAbstract: false }
           && typeToConvert.GetInterfaces().Any(IsListingResponse);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var itemType = typeToConvert.GetInterfaces().First(IsListingResponse).GetGenericArguments()[0];
        return (JsonConverter)Activator.CreateInstance(typeof(ListingResponseJsonConverter<,>).MakeGenericType(typeToConvert, itemType))!;
    }

    private static bool IsListingResponse(Type i)
        => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListingResponse<>);
}

internal sealed class ListingResponseJsonConverter<TListing, TItem> : JsonConverter<TListing>
    where TListing : class, IListingResponse<TItem>, new()
{
    // The items property is the public, settable IEnumerable<TItem> distinct from Pagination.
    // GetEnumerator-returning IEnumerator<TItem> on the type is not a property; reflecting on
    // PropertyType keeps the discovery clean.
    private static readonly PropertyInfo ItemsProperty =
        typeof(TListing).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .First(p => typeof(System.Collections.Generic.IEnumerable<TItem>).IsAssignableFrom(p.PropertyType) && p.Name != nameof(IListingResponse<TItem>.Pagination));

    private static readonly PropertyInfo PaginationProperty =
        typeof(TListing).GetProperty(nameof(IListingResponse<TItem>.Pagination))!;

    public override TListing Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        var instance = new TListing();

        var itemsName = WireName(ItemsProperty, options);
        var paginationName = WireName(PaginationProperty, options);

        if (root.TryGetProperty(itemsName, out var items) && items.ValueKind != JsonValueKind.Null)
        {
            ItemsProperty.SetValue(instance, items.Deserialize(ItemsProperty.PropertyType, options));
        }

        if (root.TryGetProperty(paginationName, out var pagination) && pagination.ValueKind != JsonValueKind.Null)
        {
            PaginationProperty.SetValue(instance, pagination.Deserialize<PaginationResponseModel>(options));
        }

        return instance;
    }

    public override void Write(Utf8JsonWriter writer, TListing value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperty(writer, WireName(ItemsProperty, options), ItemsProperty.GetValue(value), options);
        WriteProperty(writer, WireName(PaginationProperty, options), PaginationProperty.GetValue(value), options);
        writer.WriteEndObject();
    }

    private static void WriteProperty(Utf8JsonWriter writer, string name, object? value, JsonSerializerOptions options)
    {
        if (value is null && options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull)
        {
            return;
        }

        writer.WritePropertyName(name);
        JsonSerializer.Serialize(writer, value, options);
    }

    private static string WireName(PropertyInfo property, JsonSerializerOptions options)
        => property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
           ?? options.PropertyNamingPolicy?.ConvertName(property.Name)
           ?? property.Name;
}
