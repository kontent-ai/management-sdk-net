using System.Collections;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.AssetRenditions;

[JsonObject]
internal class AssetRenditionsListingResponseServerModel : IListingResponse<AssetRenditionModel>
{
    [JsonProperty("asset_renditions")]
    public IEnumerable<AssetRenditionModel> AssetRenditions { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AssetRenditionModel> GetEnumerator() => 
        AssetRenditions.GetEnumerator();
}
