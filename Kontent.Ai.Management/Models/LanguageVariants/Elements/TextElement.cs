namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a text element.</summary>
public sealed record TextElement : BaseElement
{
    /// <summary>The plain-text value.</summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }
}
