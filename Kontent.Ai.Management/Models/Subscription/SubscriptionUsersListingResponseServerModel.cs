using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;
internal class SubscriptionUserListingResponseServerModel : IListingResponse<SubscriptionUserModel>
{
    [JsonPropertyName("users")]
    public IEnumerable<SubscriptionUserModel> Users { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    public IEnumerator<SubscriptionUserModel> GetEnumerator() => Users.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
