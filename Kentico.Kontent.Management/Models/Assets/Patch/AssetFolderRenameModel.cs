using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets.Patch
{
    /// <summary>
    /// Represents rename operation to perform on the folder.
    /// </summary>
    public class AssetFolderRenameModel : AssetFolderOperationBaseModel
    {
        /// <summary>
        /// Represents the rename operation.
        /// </summary>
        public override string Op => "rename";

        /// <summary>
        /// Gets or sets the reference to the folder to be renamed.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
