namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// <c>remove</c> operation. Removes the taxonomy term identified by <see cref="TaxonomyGroupOperationBaseModel.Reference"/>.
/// </summary>
public sealed record TaxonomyGroupRemovePatchModel : TaxonomyGroupOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "remove";
}
