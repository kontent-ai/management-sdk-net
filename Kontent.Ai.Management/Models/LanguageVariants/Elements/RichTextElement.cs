namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a rich_text element: the markup plus any inline components.</summary>
public sealed record RichTextElement : BaseElement
{
    /// <summary>The rich-text markup.</summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    /// <summary>Components embedded in the rich text; omitted when null.</summary>
    [JsonPropertyName("components")]
    public IEnumerable<ComponentModel>? Components { get; init; }
}
