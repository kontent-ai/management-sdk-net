namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Project and environment identity — returned by <c>GetEnvironmentInformationAsync</c> and carried on a validation report.
/// </summary>
public sealed record EnvironmentInformationModel
{
    /// <summary>
    /// Project ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// Project name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string ProjectName { get; init; }

    /// <summary>
    /// Environment name.
    /// </summary>
    [JsonPropertyName("environment")]
    public required string EnvironmentName { get; init; }
}
