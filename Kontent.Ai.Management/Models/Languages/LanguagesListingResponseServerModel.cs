using System.Collections;

namespace Kontent.Ai.Management.Models.Languages;
internal sealed class LanguagesListingResponseServerModel : IListingResponse<LanguageModel>
{
    [JsonPropertyName("languages")]
    public required IEnumerable<LanguageModel> Languages { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<LanguageModel> GetEnumerator() => Languages.GetEnumerator();
}
