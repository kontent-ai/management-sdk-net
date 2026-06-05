using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of a number element.</summary>
public sealed record NumberElement : BaseElement
{
    /// <summary>The numeric value.</summary>
    [JsonPropertyName("value")]
    public decimal? Value { get; init; }
}
