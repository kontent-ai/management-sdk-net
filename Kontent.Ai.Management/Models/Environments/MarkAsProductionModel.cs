using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents settings of marking environment as production.
/// </summary>
public sealed record MarkAsProductionModel
{
    /// <summary>
    /// Gets a flag determining whether webhooks on the new production environment should be enabled.
    /// </summary>
    [JsonPropertyName("enable_webhooks")]
    public bool EnableWebhooks { get; init; }
}
