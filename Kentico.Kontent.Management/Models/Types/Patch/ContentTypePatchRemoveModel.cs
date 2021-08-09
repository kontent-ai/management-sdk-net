using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public class ContentTypePatchRemoveModel : ContentTypeOperationBaseModel
    {
        public override string Op => "remove";
    }
}
