using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Represents taxonomy action.
/// </summary>
public class TaxonomyActionModel
{
    /// <summary>
    /// Taxonomy action.
    /// </summary>
    [JsonProperty("action")]
    public TaxonomyActionEnum Action { get; set; }
}