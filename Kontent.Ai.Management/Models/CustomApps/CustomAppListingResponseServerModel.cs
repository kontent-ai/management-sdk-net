using System.Collections;

namespace Kontent.Ai.Management.Models.CustomApps;
internal class CustomAppListingResponseServerModel : IListingResponse<CustomAppModel>
{
    [JsonPropertyName("custom_apps")]
    public required IEnumerable<CustomAppModel> CustomApps { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<CustomAppModel> GetEnumerator() => CustomApps.GetEnumerator();
}