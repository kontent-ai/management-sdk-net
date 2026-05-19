using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Represents language action.
/// </summary>
public class LanguageActionModel
{
    /// <summary>
    /// Language action.
    /// </summary>
    [JsonPropertyName("action")]
    public LanguageAction Action { get; set; }
}