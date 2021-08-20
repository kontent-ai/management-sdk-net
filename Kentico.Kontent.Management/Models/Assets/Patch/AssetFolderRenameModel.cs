using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets.Patch
{
    public class AssetFolderRenameModel : AssetFolderOperationBaseModel
    {
        public override string Op => "rename";

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
