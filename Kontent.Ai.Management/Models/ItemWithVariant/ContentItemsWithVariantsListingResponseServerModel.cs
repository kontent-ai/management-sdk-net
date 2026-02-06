using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

[JsonObject]
internal class ContentItemsWithVariantsListingResponseServerModel : IListingResponse<ContentItemWithVariantModel>
{
    [JsonProperty("data")]
    public IEnumerable<ContentItemWithVariantModel> Data { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentItemWithVariantModel> GetEnumerator() => Data.GetEnumerator();
}
