using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public class ContentTypePatchReplaceModel : ContentTypeOperationBaseModel
    {
        public override string Op => "replace";

        [JsonProperty("value")]
        //todo make it strongly typed
        public dynamic Value { get; set; }

        [JsonProperty("before")]
        public Reference Before { get; set; }

        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
