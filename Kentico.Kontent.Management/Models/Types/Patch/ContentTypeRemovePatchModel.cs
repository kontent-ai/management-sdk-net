namespace Kentico.Kontent.Management.Models.Types.Patch
{
    /// <summary>
    /// Represents the remove operation.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type
    /// </summary>
    public class ContentTypeRemovePatchModel : ContentTypeOperationBaseModel
    {
        /// <summary>
        /// Represents the remove operation.
        /// </summary>
        public override string Op => "remove";
    }
}
