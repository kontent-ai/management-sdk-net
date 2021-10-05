using System;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents the base class of an element in the language variant.
    /// </summary>
    public abstract class BaseElement
    {
        /// <summary>
        /// Gets or sets the value of the element.
        /// </summary>
        [JsonProperty("element", Required = Required.Always)]
        public Reference Element { get; set; }

        /// <summary>
        /// Transforms the element in a language variant to the dynamic object.
        /// </summary>
        public abstract dynamic ToDynamic(Guid elementId);
    }
}