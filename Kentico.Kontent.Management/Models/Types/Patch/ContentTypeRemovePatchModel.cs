using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Patch
{
    public class ContentTypeRemovePatchModel : ContentTypeOperationBaseModel
    {
        public override string Op => "remove";
    }
}
