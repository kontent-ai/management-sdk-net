using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a custom element: the opaque payload plus its searchable plaintext companion.</summary>
public sealed record CustomElement : BaseElement
{
    /// <summary>The opaque value owned by the custom element.</summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    /// <summary>Plaintext used for search; omitted when null.</summary>
    [JsonPropertyName("searchable_value")]
    public string? SearchableValue { get; init; }
}
