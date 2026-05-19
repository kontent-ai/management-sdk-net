using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the base taxonomy model. 
/// </summary>
public record TaxonomyBaseModel
{
    /// <summary>
    /// Gets the taxonomy group's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the taxonomy group's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the taxonomy group's external ID.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
