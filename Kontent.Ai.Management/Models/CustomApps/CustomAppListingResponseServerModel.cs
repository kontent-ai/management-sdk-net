using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.CustomApps;

[JsonObject]
internal class CustomAppListingResponseServerModel : IListingResponse<CustomAppModel>
{
    [JsonProperty("custom_apps")]
    public IEnumerable<CustomAppModel> CustomApps { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<CustomAppModel> GetEnumerator() => CustomApps.GetEnumerator();
}