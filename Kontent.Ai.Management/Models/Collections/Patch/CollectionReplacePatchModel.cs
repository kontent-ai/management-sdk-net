
namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Patch operation that updates a property on an existing collection (today, only the name).
/// </summary>
public sealed record CollectionReplacePatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    public override string Op => "replace";

    /// <summary>
    /// Reference to the collection being updated.
    /// </summary>
    [JsonPropertyName("reference")]
    public required Reference Reference { get; init; }

    /// <summary>
    /// New value for the property identified by <see cref="PropertyName"/>.
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }

    /// <summary>
    /// The property to update. Defaults to <see cref="CollectionPropertyName.Name"/> — today the only supported property.
    /// </summary>
    [JsonPropertyName("property_name")]
    public CollectionPropertyName PropertyName { get; init; } = CollectionPropertyName.Name;
}
