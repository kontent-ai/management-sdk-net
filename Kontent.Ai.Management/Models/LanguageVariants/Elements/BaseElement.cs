using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Base type for typed language-variant element values. Each concrete element pairs an <see cref="Element"/> reference
/// with a strongly-typed value and serializes directly into <see cref="LanguageVariantUpsertModel.Elements"/> — the
/// typed middle tier between the raw anonymous payload and a fully generated content-type record.
/// </summary>
public abstract record BaseElement
{
    /// <summary>Reference to the content-type element this value belongs to.</summary>
    [JsonPropertyName("element")]
    public required Reference Element { get; init; }
}
