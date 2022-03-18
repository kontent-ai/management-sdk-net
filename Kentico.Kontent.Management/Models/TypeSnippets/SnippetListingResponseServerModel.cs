using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.TypeSnippets
{
    [JsonObject]
    internal class SnippetListingResponseServerModel : IListingResponse<ContentTypeSnippetModel>
    {
        [JsonProperty("snippets")]
        public IEnumerable<ContentTypeSnippetModel> Snippets { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<ContentTypeSnippetModel> GetEnumerator() => Snippets.GetEnumerator();
    }
}
