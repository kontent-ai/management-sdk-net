namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// <c>remove</c> operation. Removes the object at <see cref="ContentTypeSnippetOperationBaseModel.Path"/>.
/// </summary>
public sealed record ContentTypeSnippetRemovePatchModel : ContentTypeSnippetOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "remove";
}
