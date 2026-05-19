using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Spaces;

/// <summary>
/// Represents the space create model.
/// </summary>
public sealed record SpaceCreateModel
{
    /// <summary>
    /// Gets the space's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the space's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the space's root item.
    /// </summary>
    [JsonProperty("web_spotlight_root_item")]
    public Reference WebSpotlightRootItem { get; init; }

    /// <summary>
    /// Gets the space's collections
    /// </summary>
    [JsonProperty("collections")]
    public IEnumerable<Reference> Collections { get; init; }
}