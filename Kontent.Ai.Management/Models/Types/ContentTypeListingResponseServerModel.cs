namespace Kontent.Ai.Management.Models.Types;
internal sealed record ContentTypeListingResponseServerModel : IListingResponse<ContentTypeModel>
{
    [JsonPropertyName("types")]
    public required IEnumerable<ContentTypeModel> Types { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}

