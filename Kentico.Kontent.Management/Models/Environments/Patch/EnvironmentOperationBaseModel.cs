using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Environments.Patch
{
    /// <summary>
    /// Represents the operation on environment.
    /// </summary>
    public abstract class EnvironmentOperationBaseModel
    {
        /// <summary>
        /// Gets specification of the operation to perform.
        /// </summary>
        [JsonProperty("op", Required = Required.Always)]
        public abstract string Op { get; }
    }
}
