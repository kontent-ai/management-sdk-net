using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.Items.Elements
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
        public IEnumerable<TaxonomyTermIdentifier> Value { get; set; }

        public TaxonomyElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                // TODO - Verify if the internal type <ObjectIdentifier> is ok - maybe TaxonomyTermIdentifier would fit in DynamicObjectJsonCoverter better
                Value = (data.value as IEnumerable<dynamic>)?.Select<dynamic, TaxonomyTermIdentifier>(identifier => TaxonomyTermIdentifier.ById(Guid.Parse(identifier.id)));
            }
        }

        public override dynamic ToDynamic(string elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
            };
        }
    }
}
