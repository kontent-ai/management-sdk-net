using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public IEnumerable<NoExternalIdIdentifier> Value { get; set; }

        public MultipleChoiceElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                Value = (data.value as IEnumerable<dynamic>).Select<dynamic, NoExternalIdIdentifier>(identifier => NoExternalIdIdentifier.ById(Guid.Parse(identifier.id)));
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
