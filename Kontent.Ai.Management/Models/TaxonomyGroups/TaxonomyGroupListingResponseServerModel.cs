namespace Kontent.Ai.Management.Models.TaxonomyGroups;
internal sealed record TaxonomyGroupListingResponseServerModel
{
    [JsonPropertyName("taxonomies")]
    public required IReadOnlyList<TaxonomyGroupModel> Taxonomies { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
