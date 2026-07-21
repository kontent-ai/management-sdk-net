namespace Kontent.Ai.Management.Models.Items;
internal sealed record ContentItemListingResponseServerModel
{
    [JsonPropertyName("items")]
    public required IReadOnlyList<ContentItemModel> Items { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
