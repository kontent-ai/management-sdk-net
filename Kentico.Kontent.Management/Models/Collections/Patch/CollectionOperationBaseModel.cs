using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    /// <summary>
    /// Represents the operation on collections.
    /// </summary>
    public abstract class CollectionOperationBaseModel
    {
        /// <summary>
        /// Gets specification of the operation to perform.
        /// </summary>
        [JsonProperty("op", Required = Required.Always)]
        public abstract string Op { get; }
    }
}
