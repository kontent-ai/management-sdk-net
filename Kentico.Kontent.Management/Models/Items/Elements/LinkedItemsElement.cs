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
    /// Represents strongly typed linked items element.
    /// </summary>
    public class LinkedItemsElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of linked items element.
        /// </summary>
        [JsonProperty("value")]
        // TODO should be ContentItemIdentifier, or ContentItemVariantIdentifier
        public IEnumerable<ContentItemIdentifier> Value { get; set; }

        public LinkedItemsElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                // TODO - Verify if the internal type <ObjectIdentifier> is ok - maybe ContentItemIdentifier would fit in DynamicObjectJsonCoverter better
                Value = (data.value as IEnumerable<dynamic>)?.Select<dynamic, ContentItemIdentifier>(item => ContentItemIdentifier.ById(Guid.Parse(item.id)));
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
