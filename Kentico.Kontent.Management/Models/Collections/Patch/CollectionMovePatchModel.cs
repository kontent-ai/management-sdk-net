using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    /// <summary>
    /// Represents move operation.
    /// </summary>
    public sealed class CollectionMovePatchModel : CollectionOperationBaseModel
    {
        /// <summary>
        /// Represents move operation.
        /// </summary>
        public override string Op => "move";

        //todo naming reference vs CollectionIdentifier
        /// <summary>
        /// Gets or sets the reference of collection to move.
        /// </summary>
        [JsonProperty("reference")]
        public Reference CollectionIdentifier { get; set; }

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
