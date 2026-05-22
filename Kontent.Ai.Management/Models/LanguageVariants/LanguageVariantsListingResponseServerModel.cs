using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;
internal class LanguageVariantsListingResponseServerModel : IListingResponse<LanguageVariantModel>
{
    [JsonPropertyName("variants")]
    public IEnumerable<LanguageVariantModel> Variants { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<LanguageVariantModel> GetEnumerator() => Variants.GetEnumerator();
}
