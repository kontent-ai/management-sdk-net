using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents the text element that provides the default value to the URL slug element.
/// The dependent text element can be part of a content type snippet.
/// </summary>
public sealed record UrlSlugDependency
{
    /// <summary>
    /// Gets the content type snippet, specified as a reference, that contains the dependent text element.
    /// Note: The snippet property is not present if the text element is in the same content type.
    /// </summary>
    [JsonProperty("snippet", NullValueHandling = NullValueHandling.Ignore)]
    public Reference SnippetIdentifier { get; init; }

    /// <summary>
    /// Gets the dependent text element specified as a reference.
    /// </summary>
    [JsonProperty("element")]
    public Reference Element { get; init; }
}
