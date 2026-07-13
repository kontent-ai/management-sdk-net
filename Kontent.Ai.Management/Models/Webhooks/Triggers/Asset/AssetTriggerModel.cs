namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;

/// <summary>
/// Asset event trigger for a webhook.
/// </summary>
public sealed record AssetTriggerModel
{
    /// <summary>
    /// Whether this trigger is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Asset actions that fire the webhook.
    /// </summary>
    [JsonPropertyName("actions")]
    public IReadOnlyList<AssetActionModel>? Actions { get; init; }
}
