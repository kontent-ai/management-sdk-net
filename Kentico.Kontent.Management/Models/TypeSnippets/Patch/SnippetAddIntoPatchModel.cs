using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TypeSnippets.Patch
{
    public class SnippetAddIntoPatchModel : SnippetOperationBaseModel
    {
        public override string Op => "addInto";

        [JsonProperty("value")]
        public dynamic Value { get; set; }

        [JsonProperty("before")]
        public Reference Before { get; set; }

        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
