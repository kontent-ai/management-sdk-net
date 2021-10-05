using Newtonsoft.Json;
using System;

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

        /// <summary>
        /// Coverts the custom element to the dynamic object.
        /// </summary>
        public override dynamic ToDynamic(Guid elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
                searchable_value = SearchableValue
            };
        }
    }
}
