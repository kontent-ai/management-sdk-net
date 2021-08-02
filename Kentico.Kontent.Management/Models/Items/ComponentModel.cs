using System;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Items.Elements;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models
{
    /// <summary>
    /// Represents a rich text component model.
    /// </summary>
    public class ComponentModel
    {
        /// <summary>
        /// Gets or sets id of the content item.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets type of the component.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public ContentTypeIdentifier Type { get; set; }

        /// <summary>
        /// Gets or sets type of the component.
        /// </summary>
        // TODO validate if IEnumerable<object> would be more suitable
        [JsonProperty("elements", Required = Required.Always)]
        public IEnumerable<dynamic> Elements { get; set; }
    }
}