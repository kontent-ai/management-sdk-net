using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.AssetRenditions;
internal class AssetRenditionsListingResponseServerModel : IListingResponse<AssetRenditionModel>
{
    [JsonPropertyName("asset_renditions")]
    public IEnumerable<AssetRenditionModel> AssetRenditions { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AssetRenditionModel> GetEnumerator() =>
        AssetRenditions.GetEnumerator();
}
