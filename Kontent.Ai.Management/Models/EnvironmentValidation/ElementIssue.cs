using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Represents element issue with messages and element metadata.
/// </summary>
public sealed record ElementIssue
{
    /// <summary>
    /// Gets information about the element.
    /// </summary>
    [JsonProperty("element")]
    public Metadata Element { get; init; }

    /// <summary>
    /// Gets validation messages for the element.
    /// </summary>
    [JsonProperty("messages")]
    public List<string> Messages { get; init; }
}
