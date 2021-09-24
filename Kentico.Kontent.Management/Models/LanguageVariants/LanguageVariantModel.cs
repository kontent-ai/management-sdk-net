using System;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    /// <summary>
    /// Represents language variant model.
    /// </summary>
    public sealed class LanguageVariantModel
    {
        /// <summary>
        /// Gets or sets item of the variant.
        /// </summary>
        [JsonProperty("item")]
        public Reference Item { get; set; }

        /// <summary>
        /// Gets or sets elements of the variant.
        /// </summary>
        [JsonProperty("elements")]
        public IEnumerable<dynamic> Elements { get; set; }

        /// <summary>
        /// Gets or sets language of the variant.
        /// </summary>
        [JsonProperty("language")]
        public Reference Language { get; set; }

        /// <summary>
        /// Gets or sets last modified timestamp of the language variant.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets workflow steps of the language variant.
        /// </summary>
        [JsonProperty("workflow_step")]
        public Reference WorkflowStep { get; set; }
    }
}
