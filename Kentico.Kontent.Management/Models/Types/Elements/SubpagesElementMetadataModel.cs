using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// Represents the subpages element.
/// </summary>
public class SubpagesElementMetadataModel : ElementMetadataBase
{
    /// <summary>
    /// Gets or sets the element's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a flag determining whether the element must be filled in.
    /// </summary>
    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the element's guidelines, providing instructions on what to fill in.
    /// </summary>
    [JsonProperty("guidelines")]
    public string Guidelines { get; set; }

    /// <summary>
    /// Specifies the limitation for the number of items allowed within the element.
    /// </summary>
    [JsonProperty("item_count_limit")]
    public LimitModel ItemCountLimit { get; set; }

    /// <summary>
    /// Specifies allowed file types as an array of references to the content types.
    /// </summary>
    [JsonProperty("allowed_content_types")]
    public IEnumerable<Reference> AllowedContentTypes { get; set; }

    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    public override ElementMetadataType Type => ElementMetadataType.Subpages;
}
