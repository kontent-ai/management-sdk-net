using Kontent.Ai.Management.Models.LanguageVariants.Elements;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// A language variant of a content item (response shape), with untyped element values.
/// </summary>
public sealed record LanguageVariantModel : LanguageVariantMetadata
{
    /// <summary>
    /// Element values. Deserialized as <see cref="DynamicElement"/> — the wire carries no element-kind
    /// discriminator, so the value payload stays untyped while the envelope (element reference, sibling fields) is modeled.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IReadOnlyList<BaseElement> Elements { get; init; }
}
