using System.Collections;

namespace Kontent.Ai.Management.Models.Assets;
internal sealed class AssetListingResponseServerModel : IListingResponse<AssetModel>
{
    [JsonPropertyName("assets")]
    public required IEnumerable<AssetModel> Assets { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<AssetModel> GetEnumerator() => Assets.GetEnumerator();
}
