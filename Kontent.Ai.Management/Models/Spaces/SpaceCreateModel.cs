using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Spaces;

/// <summary>
/// Represents the space create model.
/// </summary>
public class SpaceCreateModel
{
    /// <summary>
    /// Gets or sets the space's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the space's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }
    
    /// <summary>
    /// Gets or sets the space's root item.
    /// </summary>
    [JsonProperty("web_spotlight_root_item")]
    public Reference WebSpotlightRootItem { get; set; }
}