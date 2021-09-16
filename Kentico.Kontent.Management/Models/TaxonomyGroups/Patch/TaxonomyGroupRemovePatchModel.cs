namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    /// <summary>
    /// Represents the remove operation.
    /// More info: https://docs.kontent.ai/reference/management-api-v2#operation/modify-a-taxonomy-group
    /// </summary>
    public class TaxonomyGroupRemovePatchModel : TaxonomyGroupOperationBaseModel
    {
        /// <summary>
        /// Represents the remove operation.
        /// </summary>
        public override string Op => "remove";
    }
}
