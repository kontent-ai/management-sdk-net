using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;
internal class TaxonomyGroupListingResponseServerModel : IListingResponse<TaxonomyGroupModel>
{
    [JsonPropertyName("taxonomies")]
    public IEnumerable<TaxonomyGroupModel> Taxonomies { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TaxonomyGroupModel> GetEnumerator() => Taxonomies.GetEnumerator();
}
