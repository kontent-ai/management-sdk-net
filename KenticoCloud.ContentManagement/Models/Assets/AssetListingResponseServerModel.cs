using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents asset listing response.
    /// </summary>
    [JsonObject]
    internal sealed class AssetListingResponseServerModel : IListingResponse<AssetModel>
    {
        /// <summary>
        /// Gets or sets collection of the assets.
        /// </summary>
        [JsonProperty("assets")]
        public IEnumerable<AssetModel> Assets { get; set; }

        /// <summary>
        /// Gets or sets pagination info.
        /// </summary>
        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>The <see cref="IEnumerator{AssetModel}"/> instance that can be used to iterate through the collection.</returns>
        public IEnumerator<AssetModel> GetEnumerator()
        {
            return Assets.GetEnumerator();
        }
    }
}
