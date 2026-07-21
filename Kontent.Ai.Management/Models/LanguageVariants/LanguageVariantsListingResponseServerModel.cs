namespace Kontent.Ai.Management.Models.LanguageVariants;
internal sealed record LanguageVariantsListingResponseServerModel
{
    [JsonPropertyName("variants")]
    public required IReadOnlyList<LanguageVariantModel> Variants { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
