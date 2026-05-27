using System.Collections;

namespace Kontent.Ai.Management.Models.Types;
internal class ContentTypeListingResponseServerModel : IListingResponse<ContentTypeModel>
{
    [JsonPropertyName("types")]
    public required IEnumerable<ContentTypeModel> Types { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentTypeModel> GetEnumerator() => Types.GetEnumerator();
}

