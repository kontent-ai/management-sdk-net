using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    [JsonObject]
    internal class ContentItemListingResponseServerModel : IListingResponse<ContentItemModel>
    {
        [JsonProperty("items")]
        public IEnumerable<ContentItemModel> Items { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ContentItemModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
