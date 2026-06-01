namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;

/// <summary>
/// An asset action that fires the webhook.
/// </summary>
public sealed record AssetActionModel
{
    /// <summary>
    /// The action performed on the asset.
    /// </summary>
    [JsonPropertyName("action")]
    public required AssetAction Action { get; init; }
}
