using System.Collections;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Items
{
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
}
