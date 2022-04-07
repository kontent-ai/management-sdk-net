using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Languages;

[JsonObject]
internal class LanguagesListingResponseServerModel : IListingResponse<LanguageModel>
{
    [JsonProperty("languages")]
    public IEnumerable<LanguageModel> Languages { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<LanguageModel> GetEnumerator() => Languages.GetEnumerator();
}
