using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents a rich text component model.
/// </summary>
public sealed record ComponentModel
{
    /// <summary>
    /// Gets the id of the content component.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the type of the component.
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public Reference Type { get; init; }

    /// <summary>
    /// Gets elements of the component.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<dynamic> Elements { get; init; }
}
