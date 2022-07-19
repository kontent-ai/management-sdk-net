using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents the element's multiple-choice options.
/// </summary>
public class MultipleChoiceOptionModel
{
    /// <summary>
    /// Gets or sets the multiple-choice option's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the multiple-choice option's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the multiple-choice option's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the multiple-choice option's external ID.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId { get; set; }
}
