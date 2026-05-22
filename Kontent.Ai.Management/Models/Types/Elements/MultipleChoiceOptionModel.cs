using System;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents the element's multiple-choice options.
/// </summary>
public sealed record MultipleChoiceOptionModel
{
    /// <summary>
    /// Gets the multiple-choice option's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the multiple-choice option's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the multiple-choice option's display name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the multiple-choice option's external ID.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId { get; init; }
}
