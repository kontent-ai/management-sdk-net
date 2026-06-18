namespace Kontent.Ai.Management.Models.Languages;
internal sealed record LanguagesListingResponseServerModel : IListingResponse<LanguageModel>
{
    [JsonPropertyName("languages")]
    public required IReadOnlyList<LanguageModel> Languages { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
