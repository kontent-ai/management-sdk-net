using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents space domain request model.
/// </summary>
public class SpaceDomainRequestModel
{
    /// <summary>
    /// Gets or sets the space reference.
    /// </summary>
    [JsonProperty("space")]
    public Reference Space { get; set; }

    /// <summary>
    /// Gets or sets the space domain.
    /// </summary>
    [JsonProperty("domain")]
    public string Domain { get; set; }
}