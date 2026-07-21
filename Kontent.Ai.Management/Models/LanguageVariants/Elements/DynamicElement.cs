using System.Text.Json;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Escape-hatch element value for kinds the SDK does not model as a dedicated <see cref="BaseElement"/> subtype, and the
/// carrier for re-upserting a variant's raw JSON elements. <see cref="Value"/> is serialized verbatim by its runtime
/// type; any sibling wire properties beyond <c>element</c> and <c>value</c> survive through <see cref="AdditionalData"/>.
/// </summary>
public sealed record DynamicElement : BaseElement
{
    /// <summary>
    /// The element's <c>value</c> payload, serialized by its runtime type. A <c>null</c> is written as an explicit
    /// JSON <c>null</c> (exempt from the serializer's omit-null default) so a fetched <c>"value": null</c> — an unset
    /// element — survives the re-upsert round trip instead of silently degrading to an omitted property.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public object? Value { get; init; }

    /// <summary>Wire properties other than <c>element</c> and <c>value</c> (e.g. <c>display_timezone</c>, <c>components</c>), preserved verbatim.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalData { get; init; }
}
