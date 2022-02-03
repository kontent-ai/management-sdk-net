using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    /// <summary>
    /// Represents the replace operation.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    public class ContentTypeReplacePatchModel : ContentTypeOperationBaseModel
    {
        /// <summary>
        /// Represents the replace operation.
        /// </summary>
        public override string Op => "replace";

        /// <summary>
        /// Gets or sets the value to replace into the property specified in the path where the format depends on the specific property.
        /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
        /// </summary>
        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
