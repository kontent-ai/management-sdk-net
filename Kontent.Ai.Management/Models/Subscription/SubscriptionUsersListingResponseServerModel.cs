﻿using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Subscription;

[JsonObject]
internal class SubscriptionUserListingResponseServerModel : IListingResponse<SubscriptionUserModel>
{
    [JsonProperty("users")]
    public IEnumerable<SubscriptionUserModel> Users { get; set; }

    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    public IEnumerator<SubscriptionUserModel> GetEnumerator() => Users.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
