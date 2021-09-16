using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.TypeSnippets
{
    [JsonObject]
    internal class SnippetsListingResponseServerModel : IListingResponse<ContentTypeSnippet>
    {
        [JsonProperty("snippets")]
        public IEnumerable<ContentTypeSnippet> Snippets { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ContentTypeSnippet> GetEnumerator()
        {
            return Snippets.GetEnumerator();
        }
    }
}
