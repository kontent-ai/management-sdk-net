
namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;

/// <summary>
/// Represents asset action.
/// </summary>
public class AssetActionModel
{
    /// <summary>
    /// Asset action.
    /// </summary>
    [JsonPropertyName("action")]
    public AssetAction Action { get; set; }
}