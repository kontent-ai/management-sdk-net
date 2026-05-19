using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.ItemWithVariant;
internal class ContentItemsWithVariantsListingResponseServerModel : IListingResponse<ContentItemWithVariantModel>
{
    [JsonPropertyName("data")]
    public IEnumerable<ContentItemWithVariantModel> Data { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentItemWithVariantModel> GetEnumerator() => Data.GetEnumerator();
}
