using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the strongly typed custom element.
    /// </summary>
    public class CustomElement : BaseElement
    {
        /// <summary>
        /// Gets or sets the value of the custom element.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the searchable value of the custom element.
        /// </summary>
        [JsonProperty("searchable_value")]
        public string SearchableValue { get; set; }

        /// <summary>
        /// Coverts the custom element to the dynamic object.
        /// </summary>
        public override dynamic ToDynamic()
        {
            return new
            {
                element = GetDynamicReference(),
                value = Value,
                searchable_value = SearchableValue
            };
        }
    }
}
