using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Subscription
{
    [JsonObject]
    internal class SubscriptionUserListingResponseServerModel : IListingResponse<SubscriptionUserModel>
    {
        [JsonProperty("users")]
        public IEnumerable<SubscriptionUserModel> Users { get; set; }

        [JsonProperty("pagination")]
        public PaginationResponseModel Pagination { get; set; }

        public IEnumerator<SubscriptionUserModel> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Users.GetEnumerator();
        }
    }
}
