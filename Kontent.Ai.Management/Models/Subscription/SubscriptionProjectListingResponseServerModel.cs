using System.Collections;

namespace Kontent.Ai.Management.Models.Subscription;
internal class SubscriptionProjectListingResponseServerModel : IListingResponse<SubscriptionProjectModel>
{
    [JsonPropertyName("projects")]
    public required IEnumerable<SubscriptionProjectModel> Projects { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationResponseModel Pagination { get; set; }

    public IEnumerator<SubscriptionProjectModel> GetEnumerator() => Projects.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
