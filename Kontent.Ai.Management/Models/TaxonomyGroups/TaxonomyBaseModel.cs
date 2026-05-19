using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the base taxonomy model. 
/// </summary>
public record TaxonomyBaseModel
{
    /// <summary>
    /// Gets the taxonomy group's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the taxonomy group's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the taxonomy group's external ID.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; init; }
}
