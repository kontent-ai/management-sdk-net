using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kentico.Kontent.Management.Models.Languages
{
    public class LanguagePatchModel
    {
        [JsonProperty("op")]
        public string Op => "replace";

        [JsonProperty("property_name")]
        public LanguangePropertyName PropertyName { get; set; }

        //todo make it strongly typed
        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
