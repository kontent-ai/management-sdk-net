using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets.Patch
{
    public class AssetFolderAddIntoModel : AssetFolderOperationBaseModel
    {
        public override string Op => "addInto";

        [JsonProperty("value")]
        public AssetFolderHierarchy Value { get; set; }

        [JsonProperty("before")]
        public Reference Before { get; set; }

        [JsonProperty("after")]
        public Reference After { get; set; }
    }
}
