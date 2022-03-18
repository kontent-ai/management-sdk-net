using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the subpages element.
    /// </summary>
    public class SubpagesElement : BaseElement
    {
        /// <summary>
        /// Gets or sets the value of the subpages element.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<Reference> Value { get; set; }

        /// <summary>
        /// Coverts the subpages element to the dynamic object.
        /// </summary>
        public override dynamic ToDynamic() => new {
            element = GetDynamicReference(),
            value = Value,
        };
    }
}
