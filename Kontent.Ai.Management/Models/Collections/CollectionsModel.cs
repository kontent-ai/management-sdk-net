namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Response shape for retrieving the content collection list of an environment.
/// </summary>
public sealed record CollectionsModel
{
    /// <summary>
    /// All content collections in the environment.
    /// </summary>
    [JsonPropertyName("collections")]
    public required IEnumerable<CollectionModel> Collections { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the most recent collection change. Null when collections have never been modified.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }
}
