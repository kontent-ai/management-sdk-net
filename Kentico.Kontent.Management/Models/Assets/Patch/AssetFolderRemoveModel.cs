using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets.Patch
{
    public class AssetFolderRemoveModel : AssetFolderOperationBaseModel
    {
        public override string Op => "remove";
    }
}
