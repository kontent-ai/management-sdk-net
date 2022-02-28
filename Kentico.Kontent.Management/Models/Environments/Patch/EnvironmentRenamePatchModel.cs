using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Environments.Patch
{
    /// <summary>
    /// Represents the rename operation.
    /// </summary>
    public sealed class EnvironmentRenamePatchModel : EnvironmentOperationBaseModel
    {
        /// <summary>
        /// Represents the rename_environment operation.
        /// </summary>
        public override string Op => "rename_environment";

        /// <summary>
        /// Gets or sets the environment name.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
