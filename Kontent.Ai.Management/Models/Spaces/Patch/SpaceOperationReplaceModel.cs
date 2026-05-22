
namespace Kontent.Ai.Management.Models.Spaces.Patch;

/// <summary>
/// Represents the replace operation.
/// </summary>
public sealed record SpaceOperationReplaceModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    [JsonPropertyName("op")]
    public string Op => "replace";

    /// <summary>
    /// Gets the name of the property to modify.
    /// </summary>
    [JsonPropertyName("property_name")]
    public PropertyName PropertyName { get; init; }

    /// <summary>
    /// Gets the value to insert in the specified property.
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; init; }
}