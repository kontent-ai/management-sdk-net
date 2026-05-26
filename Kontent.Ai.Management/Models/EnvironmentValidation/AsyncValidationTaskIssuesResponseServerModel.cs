using System.Collections;

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
