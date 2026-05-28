using System.Collections;

namespace Kontent.Ai.Management.Models.TypeSnippets;
internal class SnippetListingResponseServerModel : IListingResponse<ContentTypeSnippetModel>
{
    [JsonPropertyName("snippets")]
    public required IEnumerable<ContentTypeSnippetModel> Snippets { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentTypeSnippetModel> GetEnumerator() => Snippets.GetEnumerator();
}
