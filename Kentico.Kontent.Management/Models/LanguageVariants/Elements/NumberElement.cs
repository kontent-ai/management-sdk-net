using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed number element.
    /// </summary>
    public class NumberElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of the number element.
        /// </summary>
        [JsonProperty("value")]
        public decimal? Value { get; set; }

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
