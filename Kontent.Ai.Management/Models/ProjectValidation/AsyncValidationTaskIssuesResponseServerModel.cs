using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.ProjectValidation;

[JsonObject]
internal class AsyncValidationTaskIssuesResponseServerModel : IListingResponse<AsyncValidationTaskIssue>
{
    [JsonProperty("issues")]
    public IEnumerable<AsyncValidationTaskIssue> Issues { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<AsyncValidationTaskIssue> GetEnumerator() => Issues.GetEnumerator();
}
