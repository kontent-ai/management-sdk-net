using System;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models.Shared;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantResponseModel
    {
        [JsonProperty("item")]
        public ManageApiReference Item { get; set; }

        [JsonProperty("elements")]
        public IDictionary<string, object> Elements { get; set; }

        [JsonProperty("language")]
        public ManageApiReference Language { get; set; }

        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
