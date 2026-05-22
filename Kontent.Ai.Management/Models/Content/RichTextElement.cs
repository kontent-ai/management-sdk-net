
namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Value of a rich-text element on a generated content-type record. The <see cref="Value"/> string carries the
/// HTML payload; <see cref="Components"/> describes inline components, each by their generated record instance,
/// and is non-null only when components are present.
/// </summary>
/// <remarks>
/// Distinct from the legacy <c>Kontent.Ai.Management.Models.LanguageVariants.Elements.RichTextElement</c>, which
/// is bound to the Newtonsoft dynamic-elements write path. The two coexist by namespace.
/// </remarks>
public sealed record RichTextElement
{
    /// <summary>HTML payload of the rich-text element.</summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }

    /// <summary>Inline components, when present.</summary>
    [JsonPropertyName("components")]
    public IReadOnlyList<Component>? Components { get; init; }
}
