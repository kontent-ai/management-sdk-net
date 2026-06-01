namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// A language action that fires the webhook.
/// </summary>
public sealed record LanguageActionModel
{
    /// <summary>
    /// The action performed on the language.
    /// </summary>
    [JsonPropertyName("action")]
    public required LanguageAction Action { get; init; }
}
