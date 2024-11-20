namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Represents the replace operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
/// </summary>
public class CustomAppReplacePatchModel : CustomAppOperationBaseModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    public override string Op => "replace";
}