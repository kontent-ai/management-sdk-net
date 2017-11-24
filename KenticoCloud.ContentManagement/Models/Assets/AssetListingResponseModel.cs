using System.Collections.Generic;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    internal class AssetListingResponseModel
    {
        [JsonProperty("assets")]
        public IEnumerable<AssetModel> Assets { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }
    }
}
