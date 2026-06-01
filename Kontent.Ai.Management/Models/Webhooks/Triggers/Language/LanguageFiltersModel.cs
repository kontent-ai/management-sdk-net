namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Filters narrowing which languages fire a webhook.
/// </summary>
public sealed record LanguageFiltersModel
{
    /// <summary>
    /// Restrict to these languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<Reference>? Languages { get; init; }
}
