namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Represents the addInto operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-custom-app
/// </summary>
public class CustomAppAddIntoPatchModel : CustomAppOperationBaseModel
{
    /// <summary>
    /// Represents the addInto operation.
    /// </summary>
    public override string Op => "addInto";
}