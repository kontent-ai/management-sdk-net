using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TypeSnippets.Patch
{
    /// <summary>
    /// Represents the addInto operation.
    /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/modify-a-content-type-snippet
    /// </summary>
    public class ContentTypeSnippetAddIntoPatchModel : ContentTypeSnippetOperationBaseModel
    {
        /// <summary>
        /// Represents the addInto operation.
        /// </summary>
        public override string Op => "addInto";

        /// <summary>
        /// Gets or sets the object to be added. The value depends on the selected path.
        /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/modify-a-content-type-snippet
        /// </summary>
        [JsonProperty("value")]
        public dynamic Value { get; set; }

        /// <summary>
        /// Gets or sets reference of the existing object before which you want to add the new object.
        /// Note: The before and after properties are mutually exclusive.
        /// </summary>
        [JsonProperty("before")]
        public Reference Before { get; set; }

        /// <summary>
        /// Gets or sets reference of the existing object before which you want to add the new object.
        /// Note: The before and after properties are mutually exclusive.
        /// </summary>
        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
