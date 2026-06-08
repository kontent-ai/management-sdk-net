namespace Kontent.Ai.Management.Models.Subscription;
internal sealed record SubscriptionUserListingResponseServerModel : IListingResponse<SubscriptionUserModel>
{
    [JsonPropertyName("users")]
    public required IEnumerable<SubscriptionUserModel> Users { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
