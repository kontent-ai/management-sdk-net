using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed text element.
    /// </summary>
    public class TextElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of the text element.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        public override dynamic ToDynamic(Guid elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
            };
        }
    }
}
