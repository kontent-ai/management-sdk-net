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

        // TODO - add searchable value https://docs.kontent.ai/reference/management-api-v2#section/Custom-element

        public CustomElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                Value = data.value?.ToString();
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
