﻿using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantModel
    {
        [JsonProperty("item")]
        public ManageApiReference Item { get; set; }

        [JsonProperty("elements")]
        public dynamic Elements { get; set; }

        [JsonProperty("language")]
        public LanguageIdentifier Language { get; set; }

        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}