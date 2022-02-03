using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    /// <summary>
    /// Represents the addInto operation.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
    /// </summary>
    public sealed class CollectionAddIntoPatchModel : CollectionOperationBaseModel
    {
        /// <summary>
        /// Represents the addInto operation.
        /// </summary>
        public override string Op => "addInto";

        /// <summary>
        /// Gets or sets the collection to be added.
        /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
        /// </summary>
        [JsonProperty("value")]
        public CollectionCreateModel Value { get; set; }

        /// <summary>
        /// Gets or sets reference of the existing collection before which you want to add the new collection.
        /// Note: The before and after properties are mutually exclusive.
        /// </summary>
        [JsonProperty("before")]
        public Reference Before { get; set; }

        /// <summary>
        /// Gets or sets reference of the existing collection after which you want to add the new collection.
        /// Note: The before and after properties are mutually exclusive.
        /// </summary>
        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
