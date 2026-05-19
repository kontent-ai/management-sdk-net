using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Types;
internal class ContentTypeListingResponseServerModel : IListingResponse<ContentTypeModel>
{
    [JsonPropertyName("types")]
    public IEnumerable<ContentTypeModel> Types { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ContentTypeModel> GetEnumerator() => Types.GetEnumerator();
}

