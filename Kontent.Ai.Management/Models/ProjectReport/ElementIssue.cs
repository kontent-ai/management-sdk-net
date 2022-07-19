using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.ProjectReport;

/// <summary>
/// Represents element issue with messages and element metadata
/// </summary>
public sealed class ElementIssue
{
    /// <summary>
    /// Gets or sets information about the content element
    /// </summary>
    [JsonProperty("element")]
    public Metadata Element { get; set; }

    /// <summary>
    /// Gets or sets validation messages 
    /// for the content element
    /// </summary>
    [JsonProperty("messages")]
    public List<string> Messages { get; set; }
}
