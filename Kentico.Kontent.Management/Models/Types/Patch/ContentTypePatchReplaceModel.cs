using Kentico.Kontent.Management.Models.Items.Identifiers;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public class ContentTypePatchReplaceModel : ContentTypeOperationBaseModel
    {
        [JsonProperty("op")]
        public override string Op => "replace";

        [JsonProperty("value")]
        //todo make it strongly typed
        public dynamic Value { get; set; }

        [JsonProperty("before")]
        public Identifier Before { get; set; }

        [JsonProperty("after")]
        public Identifier After { get; set; }
    }
}
