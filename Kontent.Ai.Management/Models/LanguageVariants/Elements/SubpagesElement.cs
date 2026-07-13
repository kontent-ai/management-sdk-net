namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a subpages element: the linked subpages, referenced by id or codename.</summary>
public sealed record SubpagesElement : BaseElement
{
    /// <summary>The linked subpages.</summary>
    [JsonPropertyName("value")]
    public IReadOnlyList<Reference>? Value { get; init; }
}
