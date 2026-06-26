namespace Kontent.Ai.Management.Models.Subscription;
internal sealed record SubscriptionUserListingResponseServerModel
{
    [JsonPropertyName("users")]
    public required IReadOnlyList<SubscriptionUserModel> Users { get; init; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; init; }
}
