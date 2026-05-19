using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents a rich text component model.
/// </summary>
public sealed record ComponentModel
{
    /// <summary>
    /// Gets the id of the content component.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the type of the component.
    /// </summary>
    [JsonPropertyName("type")]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets elements of the component.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<object> Elements { get; init; }
}
