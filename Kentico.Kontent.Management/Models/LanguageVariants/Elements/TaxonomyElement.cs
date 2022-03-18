using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the strongly typed assets element.
    /// </summary>
    public class TaxonomyElement : BaseElement
    {
        /// <summary>
        /// Gets or sets the value of the asset element.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<Reference> Value { get; set; }

        /// <summary>
        /// Coverts the taxonomy element to the dynamic object.
        /// </summary>
        public override dynamic ToDynamic() => new {
            element = GetDynamicReference(),
            value = Value,
        };
    }
}
