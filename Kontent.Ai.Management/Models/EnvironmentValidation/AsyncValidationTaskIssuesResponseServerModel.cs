using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

[JsonObject]
internal class AsyncValidationTaskIssuesResponseServerModel : IListingResponse<AsyncValidationTaskIssueModel>
{
    [JsonProperty("issues")]
    public IEnumerable<AsyncValidationTaskIssueModel> Issues { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<AsyncValidationTaskIssueModel> GetEnumerator() => Issues.GetEnumerator();
}
