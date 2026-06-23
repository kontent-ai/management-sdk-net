namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal sealed record ItemWithVariantFilterListingResponseServerModel
{
    [JsonPropertyName("variants")]
    public required IReadOnlyList<ItemWithVariantFilterResultModel> Variants { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
