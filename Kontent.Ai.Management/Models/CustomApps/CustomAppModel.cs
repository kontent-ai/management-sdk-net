namespace Kontent.Ai.Management.Models.CustomApps;

/// <summary>
/// A custom app (response shape).
/// </summary>
public sealed record CustomAppModel
{
    /// <summary>
    /// Server-generated custom app ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// URL that hosts the custom app's UI.
    /// </summary>
    [JsonPropertyName("source_url")]
    public required string SourceUrl { get; init; }

    /// <summary>
    /// Stringified JSON configuration passed to the custom app. Null when not configured.
    /// </summary>
    [JsonPropertyName("config")]
    public string? Config { get; init; }

    /// <summary>
    /// Roles allowed to use the custom app. Always present; empty when no restriction is configured.
    /// </summary>
    [JsonPropertyName("allowed_roles")]
    public required IReadOnlyList<Reference> AllowedRoles { get; init; }

    /// <summary>
    /// How the custom app is displayed in the UI. Always present; defaults to <see cref="CustomAppDisplayMode.FullScreen"/>.
    /// </summary>
    [JsonPropertyName("display_mode")]
    public required CustomAppDisplayMode DisplayMode { get; init; }
}
