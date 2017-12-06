using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents file reference.
    /// </summary>
    public sealed class FileReferenceModel
    {
        /// <summary>
        /// Gets or sets the id of the asset.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets file reference type.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public FileReferenceTypeEnum Type { get; set; }
    }
}
