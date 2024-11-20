namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Represents the remove operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
/// </summary>
public class CustomAppRemovePatchModel : CustomAppOperationBaseModel
{
    /// <summary>
    /// Represents the remove operation.
    /// </summary>
    public override string Op => "remove";
}