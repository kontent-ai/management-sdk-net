using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public class TaxonomyGroupRemovePatchModel : TaxonomyGroupOperationBaseModel
    {
        public override string Op => "remove";
    }
}
