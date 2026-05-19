using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;
internal class AsyncValidationTaskIssuesResponseServerModel : IListingResponse<AsyncValidationTaskIssueModel>
{
    [JsonPropertyName("issues")]
    public IEnumerable<AsyncValidationTaskIssueModel> Issues { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<AsyncValidationTaskIssueModel> GetEnumerator() => Issues.GetEnumerator();
}
