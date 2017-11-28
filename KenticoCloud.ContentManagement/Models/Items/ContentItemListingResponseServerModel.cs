using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    [JsonObject]
    internal class ContentItemListingResponseServerModel : IListingResponse<ContentItemResponseModel>
    {
        [JsonProperty("items")]
        public IEnumerable<ContentItemResponseModel> Items { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ContentItemResponseModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
