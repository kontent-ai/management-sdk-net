using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Asset;

/// <summary>
/// Represents asset action.
/// </summary>
public class AssetActionModel
{
    /// <summary>
    /// Asset action.
    /// </summary>
    [JsonProperty("action")]
    public AssetActionEnum Action { get; set; }
}