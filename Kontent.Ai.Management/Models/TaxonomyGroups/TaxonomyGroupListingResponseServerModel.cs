namespace Kontent.Ai.Management.Models.TaxonomyGroups;
internal sealed record TaxonomyGroupListingResponseServerModel : IListingResponse<TaxonomyGroupModel>
{
    [JsonPropertyName("taxonomies")]
    public required IEnumerable<TaxonomyGroupModel> Taxonomies { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
