using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TypeSnippets.Patch
{
    /// <summary>
    /// Represents the replace operation.
    /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/modify-a-content-type-snippet
    /// </summary>
    public class ContentTypeSnippetPatchReplaceModel : ContentTypeSnippetOperationBaseModel
    {
        /// <summary>
        /// Represents the replace operation.
        /// </summary>
        public override string Op => "replace";

        /// <summary>
        /// Gets or sets the value to insert into the property specified in the path where the format depends on the specific property.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/modify-a-content-type-snippet
        /// </summary>
        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
