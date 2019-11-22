using Kentico.Kontent.Management.Models.Items;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents language specific description for asset.
    /// </summary>
    public sealed class AssetDescription
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
