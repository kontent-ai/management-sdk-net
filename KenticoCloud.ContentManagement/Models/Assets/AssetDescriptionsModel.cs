using KenticoCloud.ContentManagement.Models.Items;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public sealed class AssetDescriptionsModel
    {
        [JsonProperty("language", Required = Required.Always)]
        public LanguageIdentifier Language { get; set; }

        [JsonProperty("description", Required = Required.AllowNull)]
        public string Description { get; set; }
    }
}
