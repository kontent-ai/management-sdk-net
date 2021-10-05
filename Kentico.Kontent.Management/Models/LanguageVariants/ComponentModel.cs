using System;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    /// <summary>
    /// Represents a rich text component model.
    /// </summary>
    public class ComponentModel
    {
        /// <summary>
        /// Gets or sets the id of the content component.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the component.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public Reference Type { get; set; }

        /// <summary>
        /// Gets or sets elements of the component.
        /// </summary>
        [JsonProperty("elements", Required = Required.Always)]
        public IEnumerable<dynamic> Elements { get; set; }
    }
}