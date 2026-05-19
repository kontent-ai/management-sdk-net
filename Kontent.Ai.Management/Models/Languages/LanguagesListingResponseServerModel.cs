using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Languages;
internal sealed class LanguagesListingResponseServerModel : IListingResponse<LanguageModel>
{
    [JsonPropertyName("languages")]
    public IEnumerable<LanguageModel> Languages { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<LanguageModel> GetEnumerator() => Languages.GetEnumerator();
}
