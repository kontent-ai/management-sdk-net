using System.Collections;

namespace Kontent.Ai.Management.Models.Items;
internal class ContentItemListingResponseServerModel : IListingResponse<ContentItemModel>
{
    [JsonPropertyName("items")]
    public IEnumerable<ContentItemModel> Items { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentItemModel> GetEnumerator() => Items.GetEnumerator();
}
