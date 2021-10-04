using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed rich text element.
    /// </summary>
    public class RichTextElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of rich text element components.
        /// </summary>
        [JsonProperty("components")]
        public IEnumerable<ComponentModel> Components { get; set; }

        /// <summary>
        /// Gets or sets value of the rich text element.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        public override dynamic ToDynamic(Guid elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
                components = Components
            };
        }
    }
}
