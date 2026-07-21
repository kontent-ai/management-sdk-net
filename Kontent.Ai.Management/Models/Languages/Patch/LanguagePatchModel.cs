namespace Kontent.Ai.Management.Models.Languages.Patch;

/// <summary>
/// A single <c>replace</c> operation in the modify-language patch payload. The endpoint takes an array of these.
/// </summary>
public sealed record LanguagePatchModel
{
    /// <summary>
    /// Operation verb. Always <c>replace</c> — the only verb supported on the languages patch endpoint.
    /// </summary>
    [JsonPropertyName("op")]
    public string Op => "replace";

    /// <summary>
    /// Property to replace. The <c>is_default</c> property is read-only and cannot be patched, so it is not part of <see cref="LanguagePropertyName"/>.
    /// </summary>
    [JsonPropertyName("property_name")]
    public required LanguagePropertyName PropertyName { get; init; }

    /// <summary>
    /// New value. Type depends on <see cref="PropertyName"/>: <c>string</c> for <c>name</c> / <c>codename</c>, <c>bool</c> for <c>is_active</c>, <c>Reference</c> for <c>fallback_language</c>.
    /// </summary>
    [JsonPropertyName("value")]
    public required object Value { get; init; }
}
