using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    /// <summary>
    /// Represents the remove operation.
    /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/list-collections
    /// </summary>
    public sealed class CollectionRemovePatchModel : CollectionOperationBaseModel
    {
        /// <summary>
        /// Represents the remove operation.
        /// </summary>
        public override string Op => "remove";

        /// <summary>
        /// Represents the reference of the collection which should be removed.
        /// </summary>
        [JsonProperty("reference")]
        public Reference CollectionIdentifier { get; set; }
    }
}
