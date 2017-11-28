using System.Collections;
using System.Collections.Generic;


using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    [JsonObject]
    internal class AssetListingResponseServerModel : IListingResponse<AssetResponseModel>
    {
        [JsonProperty("assets")]
        public IEnumerable<AssetResponseModel> Assets { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<AssetResponseModel> GetEnumerator()
        {
            return Assets.GetEnumerator();
        }
    }
}
