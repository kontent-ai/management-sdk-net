
namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Inline component within a <see cref="RichTextValue"/>. <see cref="Id"/> threads the component to its
/// placeholder in <see cref="RichTextValue.Value"/> via the matching <c>data-id</c> attribute, while
/// <see cref="Content"/> carries the embedded content item.
/// </summary>
/// <remarks>
/// <see cref="Content"/> is <see cref="JsonIgnoreAttribute">JSON-ignored</see> by design — the component wire
/// envelope (<c>{ id, type: { codename }, elements: [...] }</c>) doesn't match a flat property bag, so the
/// envelope converter owns component (de)serialization end-to-end. It is functionally required but cannot use the
/// <c>required</c> modifier, since STJ rejects <c>required</c> properties that have no deserializable setter.
/// </remarks>
public sealed record Component
{
    /// <summary>Component identifier; must match the corresponding <c>data-id</c> in the rich-text HTML.</summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>Embedded content item; its concrete type is resolved from the component's content-type id.</summary>
    [JsonIgnore]
    public IElementsModel Content { get; init; } = null!;
}
