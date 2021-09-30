using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed url slug element.
    /// </summary>
    public class UrlSlugElement : BaseElement
    {
        /// <summary>
        /// Gets or sets mode of the url slug.
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets value of the url slug.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        public override dynamic ToDynamic(Guid elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
                mode = Mode
            };
        }
    }
}
