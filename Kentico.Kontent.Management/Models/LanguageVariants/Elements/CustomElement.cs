using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed custom element,
    /// </summary>
    public class CustomElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of the custom element.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets searchable value of the custom element.
        /// </summary>
        [JsonProperty("searchable_value")]
        public string SearchableValue { get; set; }

        public CustomElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                Value = data.value?.ToString();
                //todo SearchableValue
            }
        }

        public override dynamic ToDynamic(string elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
            };
        }
    }
}
