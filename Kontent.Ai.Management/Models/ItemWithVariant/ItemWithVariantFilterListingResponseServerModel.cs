using System.Collections;

namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal class ItemWithVariantFilterListingResponseServerModel : IListingResponse<ItemWithVariantFilterResultModel>
{
    [JsonPropertyName("variants")]
    public required IEnumerable<ItemWithVariantFilterResultModel> Variants { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ItemWithVariantFilterResultModel> GetEnumerator() => Variants.GetEnumerator();
}
