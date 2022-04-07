using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Environments;

/// <summary>
/// Represents settings of marking environment as production.
/// </summary>
public class MarkAsProductionModel
{
    /// <summary>
    /// Gets or sets a flag determining whether webhooks on the new production environment should be enabled.
    /// </summary>
    [JsonProperty("enable_webhooks")]
    public bool EnableWebhooks { get; set; }
}
