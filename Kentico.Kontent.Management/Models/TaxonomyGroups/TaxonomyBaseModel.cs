using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups
{
    /// <summary>
    /// Represents the base taxonomy model. 
    /// </summary>
    public class TaxonomyBaseModel
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
    }
}