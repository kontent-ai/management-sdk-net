using Kontent.Ai.Management.Models.Shared;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.CustomApps;
internal class CustomAppListingResponseServerModel : IListingResponse<CustomAppModel>
{
    [JsonPropertyName("custom_apps")]
    public IEnumerable<CustomAppModel> CustomApps { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<CustomAppModel> GetEnumerator() => CustomApps.GetEnumerator();
}