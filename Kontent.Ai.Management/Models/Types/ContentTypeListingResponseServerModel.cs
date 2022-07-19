using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types;

[JsonObject]
internal class ContentTypeListingResponseServerModel : IListingResponse<ContentTypeModel>
{
    [JsonProperty("types")]
    public IEnumerable<ContentTypeModel> Types { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentTypeModel> GetEnumerator() => Types.GetEnumerator();
}

