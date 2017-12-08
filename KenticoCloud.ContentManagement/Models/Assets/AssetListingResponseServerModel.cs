using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    [JsonObject]
    internal sealed class AssetListingResponseServerModel : IListingResponse<AssetModel>
    {
        [JsonProperty("assets")]
        public IEnumerable<AssetModel> Assets { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<AssetModel> GetEnumerator()
        {
            return Assets.GetEnumerator();
        }
    }
}
