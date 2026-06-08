namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal sealed record ItemWithVariantFilterListingResponseServerModel : IListingResponse<ItemWithVariantFilterResultModel>
{
    [JsonPropertyName("variants")]
    public required IEnumerable<ItemWithVariantFilterResultModel> Variants { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
