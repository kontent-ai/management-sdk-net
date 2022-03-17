using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the strongly typed url slug element.
    /// </summary>
    public class UrlSlugElement : BaseElement
    {
        /// <summary>
        /// Gets or sets the mode of the url slug.
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the value of the url slug.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Coverts the url slug element to the dynamic object.
        /// </summary>
        public override dynamic ToDynamic()
        {
            return new
            {
                element = GetDynamicReference(),
                value = Value,
                mode = Mode
            };
        }
    }
}
