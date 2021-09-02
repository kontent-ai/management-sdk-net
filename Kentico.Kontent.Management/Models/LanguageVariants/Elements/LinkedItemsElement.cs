using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
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
        public IEnumerable<Reference> Value { get; set; }

        public LinkedItemsElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                Value = (data.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(item => Reference.ById(Guid.Parse(item.id)));
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
