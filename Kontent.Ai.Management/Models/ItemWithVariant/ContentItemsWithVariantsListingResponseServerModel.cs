namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal sealed record ContentItemsWithVariantsListingResponseServerModel : IListingResponse<ContentItemWithVariantModel>
{
    [JsonPropertyName("data")]
    public required IEnumerable<ContentItemWithVariantModel> Data { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
