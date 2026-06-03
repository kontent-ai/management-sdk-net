using System.Collections;

namespace Kontent.Ai.Management.Models.Subscription;
internal class SubscriptionUserListingResponseServerModel : IListingResponse<SubscriptionUserModel>
{
    [JsonPropertyName("users")]
    public required IEnumerable<SubscriptionUserModel> Users { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    public IEnumerator<SubscriptionUserModel> GetEnumerator() => Users.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
