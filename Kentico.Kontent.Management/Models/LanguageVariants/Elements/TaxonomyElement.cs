using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed assets element.
    /// </summary>
    public class TaxonomyElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of asset element.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<Reference> Value { get; set; }

        public TaxonomyElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                Value = (data.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)));
            }
        }

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
