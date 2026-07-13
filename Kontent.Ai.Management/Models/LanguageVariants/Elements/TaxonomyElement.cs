namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a taxonomy element: the assigned terms, referenced by id or codename.</summary>
public sealed record TaxonomyElement : BaseElement
{
    /// <summary>The assigned taxonomy terms.</summary>
    [JsonPropertyName("value")]
    public IReadOnlyList<Reference>? Value { get; init; }
}
