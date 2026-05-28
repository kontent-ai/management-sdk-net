namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// <c>remove</c> operation. Removes the object at <see cref="ContentTypeOperationBaseModel.Path"/>.
/// </summary>
public sealed record ContentTypeRemovePatchModel : ContentTypeOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "remove";
}
