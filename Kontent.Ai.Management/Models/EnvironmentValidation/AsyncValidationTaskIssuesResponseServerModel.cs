namespace Kontent.Ai.Management.Models.EnvironmentValidation;
internal sealed record AsyncValidationTaskIssuesResponseServerModel
{
    [JsonPropertyName("issues")]
    public required IReadOnlyList<AsyncValidationTaskIssueModel> Issues { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
