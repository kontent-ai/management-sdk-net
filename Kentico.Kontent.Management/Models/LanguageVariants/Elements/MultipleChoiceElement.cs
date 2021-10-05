using System;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed assets element.
    /// </summary>
    public class MultipleChoiceElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of asset element.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<Reference> Value { get; set; }

        /// <summary>
        /// Coverts the mupltiple choice element to the dynamic object.
        /// </summary>
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
