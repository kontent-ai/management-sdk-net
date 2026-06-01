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
    /// Element values of the component. Each entry is a polymorphic <c>{ element, value }</c> shape.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IEnumerable<object> Elements { get; init; }
}
