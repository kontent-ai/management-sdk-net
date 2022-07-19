using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

[JsonObject]
internal class TaxonomyGroupListingResponseServerModel : IListingResponse<TaxonomyGroupModel>
{
    [JsonProperty("taxonomies")]
    public IEnumerable<TaxonomyGroupModel> Taxonomies { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TaxonomyGroupModel> GetEnumerator() => Taxonomies.GetEnumerator();
}
