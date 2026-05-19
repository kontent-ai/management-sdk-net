using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Inline component within a <see cref="RichTextElement"/>. <see cref="Id"/> threads the component to its
/// placeholder in <see cref="RichTextElement.Value"/> via the matching <c>data-id</c> attribute, while
/// <see cref="Content"/> carries the embedded content item.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Content"/> is <see cref="JsonIgnoreAttribute">JSON-ignored</see> by design — <see cref="IContentItem"/>
/// is a polymorphic marker and the wire envelope for a component (<c>{ id, type: { codename }, elements: [...] }</c>)
/// doesn't match a flat property bag. The envelope converter owns component (de)serialization end-to-end;
/// direct STJ on <see cref="Component"/> emits only <see cref="Id"/> and never populates <see cref="Content"/>.
/// </para>
/// <para>
/// <see cref="Content"/> is functionally required — every component on the wire carries an embedded item — but
/// is not declared with the <c>required</c> modifier because STJ rejects <c>required</c> properties that have
/// no deserializable setter. The envelope converter validates on write and always populates on read.
/// </para>
/// </remarks>
public sealed record Component
{
    /// <summary>Component identifier; must match the corresponding <c>data-id</c> in the rich-text HTML.</summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>Embedded content item; polymorphic by the surrounding rich-text property's <c>[AllowedTypes]</c>.</summary>
    [JsonIgnore]
    public IContentItem Content { get; init; } = null!;
}
