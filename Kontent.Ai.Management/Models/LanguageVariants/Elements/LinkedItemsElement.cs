namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a modular content (linked items) element: the linked items, referenced by id or codename.</summary>
public sealed record LinkedItemsElement : BaseElement
{
    /// <summary>The linked content items.</summary>
    [JsonPropertyName("value")]
    public IEnumerable<Reference>? Value { get; init; }
}
