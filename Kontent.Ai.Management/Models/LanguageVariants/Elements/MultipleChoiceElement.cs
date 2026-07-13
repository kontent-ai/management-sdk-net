namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a multiple_choice element: the selected options, referenced by id or codename.</summary>
public sealed record MultipleChoiceElement : BaseElement
{
    /// <summary>The selected options.</summary>
    [JsonPropertyName("value")]
    public IReadOnlyList<Reference>? Value { get; init; }
}
