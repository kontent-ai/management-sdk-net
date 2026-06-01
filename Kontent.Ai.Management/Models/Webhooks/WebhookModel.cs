using Kontent.Ai.Management.Models.Webhooks.Triggers;

namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// A webhook (response shape).
/// </summary>
public sealed record WebhookModel
{
    /// <summary>
    /// Server-generated webhook ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// URL the webhook notification is sent to.
    /// </summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    /// <summary>
    /// Secret used to sign notifications so receivers can verify they originated from Kontent.ai.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; init; }

    /// <summary>
    /// Custom HTTP headers sent with each notification. Null when none are configured.
    /// </summary>
    [JsonPropertyName("headers")]
    public IEnumerable<CustomHeaderModel>? Headers { get; init; }

    /// <summary>
    /// Whether the webhook is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public required bool Enabled { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the last change to the webhook.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }

    /// <summary>
    /// Operational health of the webhook.
    /// </summary>
    [JsonPropertyName("health_status")]
    public required WebhookHealthStatus HealthStatus { get; init; }

    /// <summary>
    /// Events that trigger the webhook.
    /// </summary>
    [JsonPropertyName("delivery_triggers")]
    public required DeliveryTriggersModel DeliveryTriggers { get; init; }
}
