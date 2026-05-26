using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents the base class for elements in types.
/// </summary>
[JsonConverter(typeof(ElementMetadataJsonConverter))]
public abstract record ElementMetadataBase
{
    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    [JsonPropertyName("type")]
    public abstract ElementMetadataType Type { get; }

    /// <summary>
    /// Gets the element's display name.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }

    /// <summary>
    /// Gets the element's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the element's codename.
    /// Unless specified, initially generated from the element's name.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the content group where the element is used in.
    /// </summary>
    [JsonPropertyName("content_group")]
    public Reference ContentGroup { get; init; }
}
