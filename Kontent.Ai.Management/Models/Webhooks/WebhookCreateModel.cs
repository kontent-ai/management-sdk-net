using Kontent.Ai.Management.Models.Webhooks.Triggers;

namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// Payload for creating a webhook.
/// </summary>
public sealed record WebhookCreateModel
{
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
    /// Secret used to sign notifications. Required — unlike the UI, the API does not auto-generate one.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; init; }

    /// <summary>
    /// Custom HTTP headers sent with each notification. Optional.
    /// </summary>
    [JsonPropertyName("headers")]
    public IEnumerable<CustomHeaderModel>? Headers { get; init; }

    /// <summary>
    /// Whether the webhook is enabled. Leave null to use the server default (enabled). A non-null value is sent verbatim.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Events that trigger the webhook.
    /// </summary>
    [JsonPropertyName("delivery_triggers")]
    public required DeliveryTriggersModel DeliveryTriggers { get; init; }
}
