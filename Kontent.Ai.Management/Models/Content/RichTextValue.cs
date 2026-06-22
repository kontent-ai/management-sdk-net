
namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Value of a rich-text element on a generated content-type record. The <see cref="Value"/> string carries the
/// HTML payload; <see cref="Components"/> describes inline components, each by their generated record instance,
/// and is non-null only when components are present.
/// </summary>
/// <remarks>Use this to set a rich_text element on a generated content-type record; its inline components are strongly-typed generated records. To set the same element by hand in the untyped element array instead, use <see cref="LanguageVariants.Elements.RichTextElement"/>.</remarks>
public sealed record RichTextValue
{
    /// <summary>HTML payload of the rich-text element.</summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }

    /// <summary>Inline components, when present.</summary>
    [JsonPropertyName("components")]
    public IEnumerable<Component>? Components { get; init; }
}
