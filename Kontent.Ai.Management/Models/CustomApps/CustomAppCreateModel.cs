namespace Kontent.Ai.Management.Models.CustomApps;

/// <summary>
/// Payload for creating a custom app.
/// </summary>
public sealed record CustomAppCreateModel
{
    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename. Required on create — the API does not auto-generate it for custom apps.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// URL that hosts the custom app's UI.
    /// </summary>
    [JsonPropertyName("source_url")]
    public required string SourceUrl { get; init; }

    /// <summary>
    /// Stringified JSON configuration passed to the custom app. Optional.
    /// </summary>
    [JsonPropertyName("config")]
    public string? Config { get; init; }

    /// <summary>
    /// Roles allowed to use the custom app. Optional.
    /// </summary>
    [JsonPropertyName("allowed_roles")]
    public IReadOnlyCollection<Reference>? AllowedRoles { get; init; }

    /// <summary>
    /// How the custom app is displayed in the UI. Optional — omit to use the server default.
    /// </summary>
    [JsonPropertyName("display_mode")]
    public CustomAppDisplayMode? DisplayMode { get; init; }
}
