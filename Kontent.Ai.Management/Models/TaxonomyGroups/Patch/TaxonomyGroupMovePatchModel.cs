namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// <c>move</c> operation. Moves the taxonomy term identified by <see cref="TaxonomyGroupOperationBaseModel.Reference"/> to a new position. The API requires exactly one of <see cref="Before"/>, <see cref="After"/>, or <see cref="Under"/>; sending none (or more than one) returns 400. <see cref="Under"/> re-parents the term to a new container.
/// </summary>
public sealed record TaxonomyGroupMovePatchModel : TaxonomyGroupOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "move";

    /// <summary>
    /// Position the moved term before this sibling. Mutually exclusive with <see cref="After"/> and <see cref="Under"/>.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference? Before { get; init; }

    /// <summary>
    /// Position the moved term after this sibling. Mutually exclusive with <see cref="Before"/> and <see cref="Under"/>.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference? After { get; init; }

    /// <summary>
    /// Re-parent the moved term as a child of this term. Mutually exclusive with <see cref="Before"/> and <see cref="After"/>.
    /// </summary>
    [JsonPropertyName("under")]
    public Reference? Under { get; init; }
}
