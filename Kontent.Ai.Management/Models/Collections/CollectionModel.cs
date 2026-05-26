namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// A single content collection within an environment.
/// </summary>
public sealed record CollectionModel
{
    /// <summary>
    /// Collection ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Collection name (1-200 chars).
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Collection codename (1-210 chars). Auto-generated from the name when not supplied on create.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }

    /// <summary>
    /// Caller-supplied external ID. Only present when one was specified on create.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }
}
