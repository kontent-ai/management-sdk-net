namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// A taxonomy term (response shape). Recursive: a term may contain child terms.
/// </summary>
public sealed record TaxonomyTermModel : TaxonomyBaseModel
{
    /// <summary>
    /// Server-generated term ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Child terms. Always present; may be empty for leaf terms.
    /// </summary>
    [JsonPropertyName("terms")]
    public required IReadOnlyList<TaxonomyTermModel> Terms { get; init; }
}
