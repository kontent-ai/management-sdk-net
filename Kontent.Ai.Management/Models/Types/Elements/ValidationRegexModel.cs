using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Specifies a regular expression pattern used to validate the text element's value.
/// </summary>
public class ValidationRegexModel
{
    /// <summary>
    /// Gets or sets the enabled state of the validation
    /// </summary>
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the regular expression used for validation
    /// </summary>
    public string Regex { get; set; }

    /// <summary>
    /// Specifies regular expression flags that affect the search. Supports only case-insensitive therefore only allowed flag is 'i'.
    /// </summary>
    public string Flags { get; set; }

    /// <summary>
    ///  Specifies the custom message that is used when input does not match the regex pattern.
    /// </summary>
    [JsonProperty("validation_message")]
    public string ValidationMessage { get; set; }
}
