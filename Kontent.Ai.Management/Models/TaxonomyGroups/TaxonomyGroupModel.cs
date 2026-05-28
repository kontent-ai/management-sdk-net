namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// A taxonomy group (response shape). Contains the root-level terms of the group.
/// </summary>
public sealed record TaxonomyGroupModel : TaxonomyBaseModel
{
    /// <summary>
    /// Server-generated taxonomy group ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the last change to the taxonomy group or any of its terms.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }

    /// <summary>
    /// Root-level terms. Always present; may be empty.
    /// </summary>
    [JsonPropertyName("terms")]
    public required IEnumerable<TaxonomyTermModel> Terms { get; init; }
}
