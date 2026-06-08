namespace Kontent.Ai.Management.Models.Subscription;
internal sealed record SubscriptionProjectListingResponseServerModel : IListingResponse<SubscriptionProjectModel>
{
    [JsonPropertyName("projects")]
    public required IEnumerable<SubscriptionProjectModel> Projects { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
