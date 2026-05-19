using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents a content type snippet element in content types.
/// </summary>
public sealed record ContentTypeSnippetElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets the element's reference to a specific content type snippet.
    /// </summary>
    [JsonProperty("snippet")]
    public Reference SnippetIdentifier { get; init; }

    /// <summary>
    /// Represents the type of the content element.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.ContentTypeSnippet;
}
