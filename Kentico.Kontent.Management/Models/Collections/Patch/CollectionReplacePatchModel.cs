using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    /// <summary>
    /// Represents the replace operation.
    /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/list-collections
    /// </summary>
    public sealed class CollectionReplacePatchModel : CollectionOperationBaseModel
    {
        /// <summary>
        /// Represents the replace operation.
        /// </summary>
        public override string Op => "replace";

        //todo naming reference (API docs) vs CollectionIdentifier
        /// <summary>
        /// Gets or sets the reference of collection which should be replaced.
        /// </summary>
        [JsonProperty("reference")]
        public Reference CollectionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the new value of the property specified in PropertyName.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/list-collections
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the property of the collection that you want to replace.
        /// Use name to change the name of the collection. Changes of other properties are currently not supported.
        /// </summary>
        [JsonProperty("property_name")]
        public PropertyName PropertyName { get; set; }
    }
}
