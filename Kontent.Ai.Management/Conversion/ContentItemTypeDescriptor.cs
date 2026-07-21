using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;
using System.Collections.Concurrent;
using System.Reflection;

namespace Kontent.Ai.Management.Conversion;

/// <summary>
/// Cached reflection metadata for a content-type record. Built lazily on first sighting of a <see cref="Type"/>
/// and cached process-wide — reflection cost is paid once per content-type.
/// </summary>
internal sealed class ContentItemTypeDescriptor
{
    private static readonly ConcurrentDictionary<Type, ContentItemTypeDescriptor> _cache = new();

    // Generated records expose collection elements as IEnumerable<T> (write-optimized); accept the common
    // read-collection interfaces and List<T> so hand-written models bind too.
    private static readonly HashSet<Type> CollectionInterfaces =
    [
        typeof(IEnumerable<>), typeof(IReadOnlyList<>), typeof(IReadOnlyCollection<>),
        typeof(IList<>), typeof(ICollection<>), typeof(List<>),
    ];

    public required Type Type { get; init; }
    public required string ContentTypeCodename { get; init; }
    public required IReadOnlyList<ContentItemPropertyDescriptor> Properties { get; init; }
    public required IReadOnlyDictionary<string, ContentItemPropertyDescriptor> ByElementId { get; init; }
    public required IReadOnlyDictionary<string, ContentItemPropertyDescriptor> ByElementCodename { get; init; }

    public static ContentItemTypeDescriptor For(Type type) => _cache.GetOrAdd(type, Build);

    private static ContentItemTypeDescriptor Build(Type type)
    {
        var typeAttr = type.GetCustomAttribute<KontentTypeAttribute>()
            ?? throw new InvalidOperationException(
                $"Type '{type.FullName}' is missing [KontentType]; the envelope converter only handles generated content-type records.");

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(BuildPropertyDescriptor)
            .OfType<ContentItemPropertyDescriptor>()
            .ToArray();

        return new ContentItemTypeDescriptor
        {
            Type = type,
            ContentTypeCodename = typeAttr.Codename,
            Properties = properties,
            // Ids are GUID strings — match them case-insensitively, like ContentTypeRegistry; codenames are
            // case-sensitive data and stay exact.
            ByElementId = properties.ToDictionary(p => p.ElementId, StringComparer.OrdinalIgnoreCase),
            ByElementCodename = properties.ToDictionary(p => p.ElementCodename),
        };
    }

    private static ContentItemPropertyDescriptor? BuildPropertyDescriptor(PropertyInfo property)
    {
        var elementAttr = property.GetCustomAttribute<KontentElementAttribute>();
        if (elementAttr is null) return null;

        var (kind, collectionElementType) = DetectKind(property.PropertyType, property);

        return new ContentItemPropertyDescriptor
        {
            Property = property,
            ElementCodename = elementAttr.Codename,
            ElementId = elementAttr.Id,
            Kind = kind,
            CollectionElementType = collectionElementType,
        };
    }

    private static (ElementKind Kind, Type? CollectionElementType) DetectKind(Type propertyType, PropertyInfo property)
    {
        var inner = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (inner == typeof(string)) return (ElementKind.Text, null);
        if (inner == typeof(decimal)) return (ElementKind.Number, null);
        if (inner == typeof(DateTimeValue)) return (ElementKind.DateTime, null);
        if (inner == typeof(UrlSlugValue)) return (ElementKind.UrlSlug, null);
        if (inner == typeof(CustomValue)) return (ElementKind.Custom, null);
        if (inner == typeof(RichTextValue)) return (ElementKind.RichText, null);

        if (inner.IsGenericType && CollectionInterfaces.Contains(inner.GetGenericTypeDefinition()))
        {
            var elem = inner.GetGenericArguments()[0];
            if (elem.IsEnum) return (ElementKind.MultipleChoice, elem);
            if (elem == typeof(AssetReference)) return (ElementKind.Asset, elem);
            if (elem == typeof(Reference)) return (ElementKind.Reference, elem);
        }

        throw new NotSupportedException(
            $"Property '{property.DeclaringType?.Name}.{property.Name}' has type '{propertyType}', which is not a recognized element kind.");
    }
}

internal sealed class ContentItemPropertyDescriptor
{
    public required PropertyInfo Property { get; init; }
    public required string ElementCodename { get; init; }
    public required string ElementId { get; init; }
    public required ElementKind Kind { get; init; }

    /// <summary>For <see cref="ElementKind.MultipleChoice"/> / <see cref="ElementKind.Asset"/> / <see cref="ElementKind.Reference"/> properties — the <c>T</c> in <c>IReadOnlyList&lt;T&gt;</c>.</summary>
    public Type? CollectionElementType { get; init; }
}

/// <summary>
/// Cached reflection metadata for an <c>[KontentEnumValue]</c>-annotated enum (multiple-choice element). Maps
/// codename and option-id back to the boxed enum value, and the value back to a codename for writes.
/// </summary>
internal sealed class EnumDescriptor
{
    private static readonly ConcurrentDictionary<Type, EnumDescriptor> _cache = new();

    public required IReadOnlyDictionary<string, object> ByCodename { get; init; }
    public required IReadOnlyDictionary<string, object> ByItemId { get; init; }
    public required IReadOnlyDictionary<object, string> CodenameByValue { get; init; }

    public static EnumDescriptor For(Type enumType) => _cache.GetOrAdd(enumType, Build);

    private static EnumDescriptor Build(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException($"Type '{enumType.FullName}' is not an enum.", nameof(enumType));
        }

        var byCodename = new Dictionary<string, object>();
        var byItemId = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        var codenameByValue = new Dictionary<object, string>();

        foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<KontentEnumValueAttribute>();
            if (attr is null) continue;

            var value = field.GetValue(null)!;
            byCodename[attr.Codename] = value;
            byItemId[attr.Id] = value;
            codenameByValue[value] = attr.Codename;
        }

        return new EnumDescriptor
        {
            ByCodename = byCodename,
            ByItemId = byItemId,
            CodenameByValue = codenameByValue,
        };
    }
}
