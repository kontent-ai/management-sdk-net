using System.Collections;

namespace Kontent.Ai.Management.Models.AssetRenditions;
internal class AssetRenditionsListingResponseServerModel : IListingResponse<AssetRenditionModel>
{
    [JsonPropertyName("asset_renditions")]
    public required IEnumerable<AssetRenditionModel> AssetRenditions { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AssetRenditionModel> GetEnumerator() =>
        AssetRenditions.GetEnumerator();
}
