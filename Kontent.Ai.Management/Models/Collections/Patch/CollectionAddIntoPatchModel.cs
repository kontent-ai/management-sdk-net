
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Patch operation that adds a new collection to the environment.
/// </summary>
public sealed record CollectionAddIntoPatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents the addInto operation.
    /// </summary>
    public override string Op => "addInto";

    /// <summary>
    /// The collection to add.
    /// </summary>
    [JsonPropertyName("value")]
    public required CollectionCreateModel Value { get; init; }

    /// <summary>
    /// Reference to the existing collection before which the new collection should be inserted. Mutually exclusive with <see cref="After"/>. When both are omitted the new collection is appended at the end.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference? Before { get; init; }

    /// <summary>
    /// Reference to the existing collection after which the new collection should be inserted. Mutually exclusive with <see cref="Before"/>. When both are omitted the new collection is appended at the end.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference? After { get; init; }
}
