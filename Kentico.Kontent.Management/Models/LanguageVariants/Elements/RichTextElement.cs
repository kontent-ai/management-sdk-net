using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the strongly typed rich text element.
    /// </summary>
    public class RichTextElement : BaseElement
    {
        /// <summary>
        /// Gets or sets the value of rich text element components.
        /// </summary>
        [JsonProperty("components")]
        public IEnumerable<ComponentModel> Components { get; set; }

        /// <summary>
        /// Gets or sets the value of the rich text element.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Coverts the rich text element to the dynamic object.
        /// </summary>
        public override dynamic ToDynamic() => new {
            element = GetDynamicReference(),
            value = Value,
            components = Components
        };
    }
}
