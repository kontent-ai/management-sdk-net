using System;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Tests.Data
{
    internal class SimpleTestModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
