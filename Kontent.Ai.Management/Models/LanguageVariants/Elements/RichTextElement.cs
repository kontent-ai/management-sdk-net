namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a rich_text element: the markup plus any inline components.</summary>
/// <remarks>Use this to set a rich_text element by hand in the untyped element array; <c>Element</c> says which element it targets. With a generated content-type record, set the element via <see cref="Content.RichTextValue"/> instead.</remarks>
public sealed record RichTextElement : BaseElement
{
    /// <summary>The rich-text markup.</summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    /// <summary>Components embedded in the rich text; omitted when null.</summary>
    [JsonPropertyName("components")]
    public IEnumerable<ComponentModel>? Components { get; init; }
}
