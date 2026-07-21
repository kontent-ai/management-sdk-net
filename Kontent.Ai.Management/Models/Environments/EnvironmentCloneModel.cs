namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Settings for cloning an environment.
/// </summary>
public sealed record EnvironmentCloneModel
{
    /// <summary>
    /// Name of the new environment.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Role IDs whose assigned users are activated in the cloned environment. Optional.
    /// </summary>
    [JsonPropertyName("roles_to_activate")]
    public IReadOnlyList<Guid>? RolesToActivate { get; init; }

    /// <summary>
    /// Options controlling which data is copied into the clone. Optional.
    /// </summary>
    [JsonPropertyName("copy_data_options")]
    public CopyDataOptions? CopyDataOptions { get; init; }
}
