using System.Collections;

namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal class ItemWithVariantFilterListingResponseServerModel : IListingResponse<ItemWithVariantFilterResultModel>
{
    [JsonPropertyName("variants")]
    public IEnumerable<ItemWithVariantFilterResultModel> Variants { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ItemWithVariantFilterResultModel> GetEnumerator() => Variants.GetEnumerator();
}
