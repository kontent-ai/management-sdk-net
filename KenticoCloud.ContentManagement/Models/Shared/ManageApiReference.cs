﻿using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models
{
    public sealed class ManageApiReference
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CodeName { get; set; }

        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}