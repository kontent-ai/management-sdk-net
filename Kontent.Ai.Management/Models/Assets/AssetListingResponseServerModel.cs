using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Assets;
internal sealed class AssetListingResponseServerModel : IListingResponse<AssetModel>
{
    [JsonPropertyName("assets")]
    public IEnumerable<AssetModel> Assets { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<AssetModel> GetEnumerator() => Assets.GetEnumerator();
}
