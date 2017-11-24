using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement
{
    public sealed class ContentItemsResponseModel
    {
        [JsonProperty("items")]
        public IEnumerable<ContentItemResponseModel> Items { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }
    }
}
