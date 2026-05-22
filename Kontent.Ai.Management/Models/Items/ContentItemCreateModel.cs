
namespace Kontent.Ai.Management.Models.Items;

/// <summary>
/// Represents the content item create model.
/// </summary>
public sealed record ContentItemCreateModel
{
    /// <summary>
    /// Gets the name of the content item.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the content item.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the type of the content item.
    /// </summary>
    [JsonPropertyName("type")]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets the external identifier of the content item.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the collection of the content item.
    /// </summary>
    [JsonPropertyName("collection")]
    public Reference Collection { get; init; }
}
