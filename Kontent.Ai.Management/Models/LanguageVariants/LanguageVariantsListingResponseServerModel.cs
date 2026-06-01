using System.Collections;

namespace Kontent.Ai.Management.Models.LanguageVariants;
internal class LanguageVariantsListingResponseServerModel : IListingResponse<LanguageVariantModel>
{
    [JsonPropertyName("variants")]
    public required IEnumerable<LanguageVariantModel> Variants { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<LanguageVariantModel> GetEnumerator() => Variants.GetEnumerator();
}
