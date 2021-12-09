using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Specifies a regular expression pattern used to validate the text element's value.
    /// </summary>
    public class ValidationRegexModel
    {
        /// <summary>
        /// Gets or sets the regular expression used for validation
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// Specifies regular expression flags that affect the search. Supports only case-insensitive therefore only allowed flag is 'i'.
        /// </summary>
        public string Flags { get; set; }
        
        /// <summary>
        ///  Specifies the custom message that is used when 
        /// </summary>
        [JsonProperty("validation_message")]
        public string ValidationMessage { get; set; }
    }
}
