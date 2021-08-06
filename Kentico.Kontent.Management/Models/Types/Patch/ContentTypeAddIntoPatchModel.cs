using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public class ContentTypeAddIntoPatchModel : ContentTypeOperationBaseModel
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
