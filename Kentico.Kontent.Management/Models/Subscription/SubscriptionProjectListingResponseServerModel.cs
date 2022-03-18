using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Subscription;

[JsonObject]
internal class SubscriptionProjectListingResponseServerModel : IListingResponse<SubscriptionProjectModel>
{
    [JsonProperty("projects")]
    public IEnumerable<SubscriptionProjectModel> Projects { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    public IEnumerator<SubscriptionProjectModel> GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Projects.GetEnumerator();
}
