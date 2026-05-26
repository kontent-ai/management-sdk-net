namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy group model.
/// </summary>
public sealed record TaxonomyGroupModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets the taxonomy group's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets ISO-8601 formatted date/time of the last change to the taxonomy group or its terms.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }

    /// <summary>
    /// Gets terms in the taxonomy group.
    /// </summary>
    [JsonPropertyName("terms")]
    public IEnumerable<TaxonomyTermModel> Terms { get; init; }
}
