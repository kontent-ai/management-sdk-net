using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Assets;

[JsonObject]
internal sealed class AssetListingResponseServerModel : IListingResponse<AssetModel>
{
    [JsonProperty("assets")]
    public IEnumerable<AssetModel> Assets { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<AssetModel> GetEnumerator() => Assets.GetEnumerator();
}
