namespace Kontent.Ai.Management.Models.LanguageVariants;
internal sealed record LanguageVariantsListingResponseServerModel : IListingResponse<LanguageVariantModel>
{
    [JsonPropertyName("variants")]
    public required IReadOnlyList<LanguageVariantModel> Variants { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
