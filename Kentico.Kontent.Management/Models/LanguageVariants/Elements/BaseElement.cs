using System;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the base class of element in languanage variant.
    /// </summary>
    public abstract class BaseElement
    {
        /// <summary>
        /// Gets or sets value of the element.
        /// </summary>
        [JsonProperty("element", Required = Required.Always)]
        public ObjectIdentifier Element { get; set; }

        // TODO Is there a way to force the inherited classes to implement constructor wit dynamic parameter?
        /// <summary>
        /// Creates reference of the element.
        /// </summary>
        public BaseElement(dynamic data = null)
        {
            // TODO extend by codename + external ID
            if (data?.element?.id != null)
            {
                Element = ObjectIdentifier.ById(Guid.Parse(data.element.id));
            }
        }

        /// <summary>
        /// Transforms the element in the language variant to dynamic object.
        /// </summary>
        public abstract dynamic ToDynamic(string elementId);
    }
}