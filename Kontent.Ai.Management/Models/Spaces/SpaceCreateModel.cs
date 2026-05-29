using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
    /// If both <see cref="RootItem"/> and <see cref="WebSpotlightRootItem"/> are set, <see cref="RootItem"/> takes precedence.
    /// </summary>
    [JsonProperty("root_item")]
    public Reference RootItem { get; set; }

    /// <summary>
    /// Gets or sets the space's root item.
    /// If both <see cref="RootItem"/> and <see cref="WebSpotlightRootItem"/> are set, <see cref="RootItem"/> takes precedence.
    /// </summary>
    [Obsolete("Use root_item instead.")]
    [JsonProperty("web_spotlight_root_item")]
    public Reference WebSpotlightRootItem { get; set; }

    /// <summary>
    /// Gets or sets the space's collections
    /// </summary>
    [JsonProperty("collections")]
    public IEnumerable<Reference> Collections { get; set; }
}