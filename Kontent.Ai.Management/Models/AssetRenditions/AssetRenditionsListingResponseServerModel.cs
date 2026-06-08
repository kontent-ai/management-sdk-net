namespace Kontent.Ai.Management.Models.AssetRenditions;
internal sealed record AssetRenditionsListingResponseServerModel : IListingResponse<AssetRenditionModel>
{
    [JsonPropertyName("asset_renditions")]
    public required IEnumerable<AssetRenditionModel> AssetRenditions { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
