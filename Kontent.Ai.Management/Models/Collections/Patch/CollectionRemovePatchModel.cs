
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Patch operation that deletes an existing collection. The collection must contain no items, and the default collection cannot be deleted.
/// </summary>
public sealed record CollectionRemovePatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents the remove operation.
    /// </summary>
    public override string Op => "remove";

    /// <summary>
    /// Reference to the collection to remove.
    /// </summary>
    [JsonPropertyName("reference")]
    public required Reference Reference { get; init; }
}
