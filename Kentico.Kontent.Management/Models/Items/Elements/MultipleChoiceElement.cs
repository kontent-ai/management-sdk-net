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
    public class MultipleChoiceElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of asset element.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<MultipleChoiceOptionIdentifier> Value { get; set; }

        public MultipleChoiceElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                // TODO - Verify if the internal type <ObjectIdentifier> is ok - maybe TaxonomyTermIdentifier would fit in DynamicObjectJsonCoverter better
                Value = (data.value as IEnumerable<dynamic>).Select<dynamic, MultipleChoiceOptionIdentifier>(identifier => MultipleChoiceOptionIdentifier.ById(Guid.Parse(identifier.id)));
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
