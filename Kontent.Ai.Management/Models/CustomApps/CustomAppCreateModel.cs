using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.CustomApps;

/// <summary>
/// Represents the custom app create model.
/// </summary>
public class CustomAppCreateModel
{
    /// <summary>
    /// Gets or sets the custom app's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets or sets the custom app's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets or sets the custom app's source url.
    /// </summary>
    [JsonProperty("source_url")]
    public string SourceUrl { get; init; }

    /// <summary>
    /// Gets or sets the custom app's config.
    /// </summary>
    [JsonProperty("config")]
    public string Config { get; init; }

    /// <summary>
    /// Gets or sets the custom app's allowed roles.
    /// </summary>
    [JsonProperty("allowed_roles")]
    public IReadOnlyCollection<Reference> AllowedRoles { get; set; }

    /// <summary>
    /// Gets or sets the custom app's display mode.
    /// </summary>
    [JsonProperty("display_mode")]
    public CustomAppDisplayMode DisplayMode { get; init; }
}