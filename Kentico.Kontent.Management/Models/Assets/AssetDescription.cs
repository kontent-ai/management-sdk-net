using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents the language specific description for the asset.
    /// </summary>
    public sealed class AssetDescription
    {
        /// <summary>
        /// Gets or sets the identifier of the language.
        /// </summary>
        [JsonProperty("language", Required = Required.Always)]
        public Reference Language { get; set; }

        /// <summary>
        /// Gets or sets the description of the asset.
        /// </summary>
        [JsonProperty("description", Required = Required.AllowNull)]
        public string Description { get; set; }
    }
}
