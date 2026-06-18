namespace Kontent.Ai.Management.Models.Assets;
internal sealed record AssetListingResponseServerModel : IListingResponse<AssetModel>
{
    [JsonPropertyName("assets")]
    public required IReadOnlyList<AssetModel> Assets { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
