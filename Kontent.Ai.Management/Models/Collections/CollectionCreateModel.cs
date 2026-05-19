using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Represents collection the create model.
/// </summary>
public sealed record CollectionCreateModel
{
    /// <summary>
    /// Gets the name of the content collection.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the codename of the collection.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the external identifier of the content collection.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
