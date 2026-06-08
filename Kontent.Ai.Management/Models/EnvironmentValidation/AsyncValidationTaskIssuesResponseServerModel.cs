namespace Kontent.Ai.Management.Models.EnvironmentValidation;
internal sealed record AsyncValidationTaskIssuesResponseServerModel : IListingResponse<AsyncValidationTaskIssueModel>
{
    [JsonPropertyName("issues")]
    public required IEnumerable<AsyncValidationTaskIssueModel> Issues { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
