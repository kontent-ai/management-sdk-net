using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

[JsonObject]
internal class ItemWithVariantFilterListingResponseServerModel : IListingResponse<ItemWithVariantFilterResultModel>
{
    [JsonProperty("variants")]
    public IEnumerable<ItemWithVariantFilterResultModel> Variants { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ItemWithVariantFilterResultModel> GetEnumerator() => Variants.GetEnumerator();
}
