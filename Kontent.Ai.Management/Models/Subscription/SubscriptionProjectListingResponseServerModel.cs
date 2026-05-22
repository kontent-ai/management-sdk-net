using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;
internal class SubscriptionProjectListingResponseServerModel : IListingResponse<SubscriptionProjectModel>
{
    [JsonPropertyName("projects")]
    public IEnumerable<SubscriptionProjectModel> Projects { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    public IEnumerator<SubscriptionProjectModel> GetEnumerator() => Projects.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
