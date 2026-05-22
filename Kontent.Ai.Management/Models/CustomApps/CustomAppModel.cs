using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.CustomApps;

/// <summary>
/// Represents the custom app model.
/// </summary>
public sealed record CustomAppModel
{
    /// <summary>
    /// Gets the custom app's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

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