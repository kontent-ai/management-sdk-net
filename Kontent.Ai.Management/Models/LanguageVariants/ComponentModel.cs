using Kontent.Ai.Management.Models.LanguageVariants.Elements;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// An inline content component embedded in a rich text element.
/// </summary>
public sealed record ComponentModel
{
    /// <summary>
    /// Component ID. Threads the component to its placeholder in the rich text via the matching <c>data-id</c> attribute.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Reference to the content type the component is based on.
    /// </summary>
    [JsonPropertyName("type")]
    public required Reference Type { get; init; }

    /// <summary>
    /// Element values of the component. Use a typed <see cref="BaseElement"/> subtype per element kind, or
    /// <see cref="DynamicElement"/> for kinds the SDK does not model.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IEnumerable<BaseElement> Elements { get; init; }
}
