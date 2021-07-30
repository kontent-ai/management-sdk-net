using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public class ContentTypePatchRemoveModel : ContentTypeOperationBaseModel
    {
        [JsonProperty("op")]
        public override string Op => "remove";
    }
}
