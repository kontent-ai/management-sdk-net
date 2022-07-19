using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;

[JsonObject]
internal class LanguageVariantsListingResponseServerModel : IListingResponse<LanguageVariantModel>
{
    [JsonProperty("variants")]
    public IEnumerable<LanguageVariantModel> Variants { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<LanguageVariantModel> GetEnumerator() => Variants.GetEnumerator();
}
