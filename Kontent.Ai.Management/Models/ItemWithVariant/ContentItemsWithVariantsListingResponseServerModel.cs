namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal sealed record ContentItemsWithVariantsListingResponseServerModel
{
    [JsonPropertyName("data")]
    public required IReadOnlyList<ContentItemWithVariantModel> Data { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
