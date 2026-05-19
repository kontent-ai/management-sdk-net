using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Languages;

/// <summary>
/// Represents the replace operation on languages.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-language
/// </summary>
public sealed record LanguagePatchModel
{
    /// <summary>
    /// Represents the replace operation.
    /// </summary>
    [JsonPropertyName("op")]
    public string Op => "replace";

    /// <summary>
    /// Gets the name of the language property that you want to modify.
    /// Enum: "name" "codename" "fallback_language" "is_active"
    /// </summary>
    [JsonPropertyName("property_name")]
    public LanguagePropertyName PropertyName { get; init; }

    /// <summary>
    /// Gets the value or object to insert in the specified property. The format of the value property depends on the value of the property_name property.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-language
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; init; }
}
