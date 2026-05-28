using System.Collections;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;
internal class TaxonomyGroupListingResponseServerModel : IListingResponse<TaxonomyGroupModel>
{
    [JsonPropertyName("taxonomies")]
    public required IEnumerable<TaxonomyGroupModel> Taxonomies { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TaxonomyGroupModel> GetEnumerator() => Taxonomies.GetEnumerator();
}
