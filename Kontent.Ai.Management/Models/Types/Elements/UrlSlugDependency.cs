namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Points a URL slug element at the text element that feeds its default value.
/// </summary>
public sealed record UrlSlugDependency
{
    /// <summary>
    /// Reference to the snippet that holds the dependent text element. Null when the text element lives on the same content type.
    /// </summary>
    [JsonPropertyName("snippet")]
    public Reference? Snippet { get; init; }

    /// <summary>
    /// Reference to the dependent text element.
    /// </summary>
    [JsonPropertyName("element")]
    public required Reference Element { get; init; }
}
