using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public class TaxonomyGroupPatchRemoveModel : TaxonomyGroupOperationBaseModel
    {
        public override string Op => "remove";
    }
}
