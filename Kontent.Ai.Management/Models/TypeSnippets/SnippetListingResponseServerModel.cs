using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TypeSnippets;
internal class SnippetListingResponseServerModel : IListingResponse<ContentTypeSnippetModel>
{
    [JsonPropertyName("snippets")]
    public IEnumerable<ContentTypeSnippetModel> Snippets { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentTypeSnippetModel> GetEnumerator() => Snippets.GetEnumerator();
}
