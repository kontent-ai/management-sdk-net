using KenticoCloud.ContentManagement.Models.Items;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents language specifi description for asset.
    /// </summary>
    public sealed class AssetDescriptionsModel
    {
        /// <summary>
        /// Gets or sets identifier of the language.
        /// </summary>
        [JsonProperty("language", Required = Required.Always)]
        public LanguageIdentifier Language { get; set; }

        /// <summary>
        /// Gets or sets Description of the asset.
        /// </summary>
        [JsonProperty("description", Required = Required.AllowNull)]
        public string Description { get; set; }
    }
}
