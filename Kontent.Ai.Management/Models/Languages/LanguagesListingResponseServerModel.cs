﻿using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Languages;

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
