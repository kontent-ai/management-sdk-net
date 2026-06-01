namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Language event trigger for a webhook.
/// </summary>
public sealed record LanguageTriggerModel
{
    /// <summary>
    /// Whether this trigger is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>
    /// Language actions that fire the webhook.
    /// </summary>
    [JsonPropertyName("actions")]
    public IEnumerable<LanguageActionModel>? Actions { get; init; }

    /// <summary>
    /// Filters narrowing which languages fire the webhook.
    /// </summary>
    [JsonPropertyName("filters")]
    public LanguageFiltersModel? Filters { get; init; }
}
