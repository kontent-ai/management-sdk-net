namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Settings applied when marking an environment as production.
/// </summary>
public sealed record MarkAsProductionModel
{
    /// <summary>
    /// Whether webhooks on the new production environment are enabled. Leave null to use the server default (enabled); a non-null value is sent verbatim.
    /// </summary>
    [JsonPropertyName("enable_webhooks")]
    public bool? EnableWebhooks { get; init; }
}
