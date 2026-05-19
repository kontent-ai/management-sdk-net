using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Taxonomy;

/// <summary>
/// Represents taxonomy action.
/// </summary>
public class TaxonomyActionModel
{
    /// <summary>
    /// Taxonomy action.
    /// </summary>
    [JsonPropertyName("action")]
    public TaxonomyAction Action { get; set; }
}