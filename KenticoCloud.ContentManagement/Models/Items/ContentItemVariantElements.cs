using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantElements
    {
        [JsonProperty("elements")]
        public Dictionary<string, string> Elements { get; private set; }

        public ContentItemVariantElements(Dictionary<string, string> elements)
        {
            Elements = elements;
        }
    }
}
