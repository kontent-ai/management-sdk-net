namespace Kontent.Ai.Management.Models.CustomApps;
internal sealed record CustomAppListingResponseServerModel : IListingResponse<CustomAppModel>
{
    [JsonPropertyName("custom_apps")]
    public required IReadOnlyList<CustomAppModel> CustomApps { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}