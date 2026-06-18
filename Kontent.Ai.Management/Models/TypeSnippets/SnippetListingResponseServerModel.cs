namespace Kontent.Ai.Management.Models.TypeSnippets;
internal sealed record SnippetListingResponseServerModel : IListingResponse<ContentTypeSnippetModel>
{
    [JsonPropertyName("snippets")]
    public required IReadOnlyList<ContentTypeSnippetModel> Snippets { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
