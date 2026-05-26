
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Patch operation that changes a collection's position in the environment's ordered collection list.
/// </summary>
public sealed record CollectionMovePatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Reference to the collection being moved.
    /// </summary>
    [JsonPropertyName("reference")]
    public required Reference Reference { get; init; }

    /// <summary>
    /// Reference to the existing collection before which the moved collection should be placed. Mutually exclusive with <see cref="After"/>.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference? Before { get; init; }

    /// <summary>
    /// Reference to the existing collection after which the moved collection should be placed. Mutually exclusive with <see cref="Before"/>.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference? After { get; init; }
}
