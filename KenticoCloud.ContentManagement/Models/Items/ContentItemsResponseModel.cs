using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Shared;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemsResponseModel
    {
        [JsonProperty("items")]
        public IEnumerable<ContentItemResponseModel> Items { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }
    }
}
