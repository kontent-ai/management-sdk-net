using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Items.Elements
{
    /// <summary>
    /// Represents strongly typed rich text element.
    /// </summary>
    public class RichTextElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of rich text element components.
        /// </summary>
        [JsonProperty("components")]
        // TODO should be an custom Component Object
        public IEnumerable<ComponentModel> Components { get; set; }

        /// <summary>
        /// Gets or sets value of the rich text element.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        public RichTextElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                Value = data.value;
                Components = (data.components as IEnumerable<dynamic>)?.Select(component => new ComponentModel
                {
                    Id = Guid.Parse(component.id),
                    Type = ContentTypeIdentifier.ById(Guid.Parse(component.type.id)),
                    // TODO - probably use reflection for constructor
                    Elements = (component.elements as IEnumerable<dynamic>)
                });
            }
        }

        public override dynamic ToDynamic(string elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
                components = Components
            };
        }
    }
}
