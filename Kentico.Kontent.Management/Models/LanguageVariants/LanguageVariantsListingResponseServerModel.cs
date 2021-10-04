using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    [JsonObject]
    internal class LanguageVariantsListingResponseServerModel : IListingResponse<LanguageVariantModel>
    {
        [JsonProperty("variants")]
        public IEnumerable<LanguageVariantModel> Variants { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<LanguageVariantModel> GetEnumerator()
        {
            return Variants.GetEnumerator();
        }
    }
}
