using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Items;

[JsonObject]
internal class ContentItemListingResponseServerModel : IListingResponse<ContentItemModel>
{
    [JsonProperty("items")]
    public IEnumerable<ContentItemModel> Items { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentItemModel> GetEnumerator() => Items.GetEnumerator();
}
