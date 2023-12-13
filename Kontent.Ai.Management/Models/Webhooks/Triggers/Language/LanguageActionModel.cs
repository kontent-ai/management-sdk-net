using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Represents language action.
/// </summary>
public class LanguageActionModel
{
    /// <summary>
    /// Language action.
    /// </summary>
    [JsonProperty("action")]
    public LanguageActionEnum Action { get; set; }
}