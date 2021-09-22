using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups
{
    /// <summary>
    /// Represents the taxonomy group create model.
    /// </summary>
    public class TaxonomyGroupCreateModel
    {
        /// <summary>
        /// Gets or sets the taxonomy group's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the taxonomy group's codename.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets the taxonomy group's external ID.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets terms in the taxonomy group.
        /// </summary>
        [JsonProperty("terms")]
        public IEnumerable<TaxonomyGroupCreateModel> Terms { get; set; }
    }
}
