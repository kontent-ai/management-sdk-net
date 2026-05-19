using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.CustomApps;

/// <summary>
/// Represents the custom app create model.
/// </summary>
public sealed record CustomAppCreateModel
{
    /// <summary>
    /// Gets the custom app's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the custom app's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the custom app's source url.
    /// </summary>
    [JsonPropertyName("source_url")]
    public string SourceUrl { get; init; }

    /// <summary>
    /// Gets the custom app's config.
    /// </summary>
    [JsonPropertyName("config")]
    public string Config { get; init; }

    /// <summary>
    /// Gets the custom app's allowed roles.
    /// </summary>
    [JsonPropertyName("allowed_roles")]
    public IReadOnlyCollection<Reference> AllowedRoles { get; init; }

    /// <summary>
    /// Gets the custom app's display mode.
    /// </summary>
    [JsonPropertyName("display_mode")]
    public CustomAppDisplayMode DisplayMode { get; init; }
}