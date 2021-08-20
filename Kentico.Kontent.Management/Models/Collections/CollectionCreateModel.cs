using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections
{
    public class CollectionCreateModel
    {
        /// <summary>
        /// Gets or sets name of the content collection.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets codename of the collection.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets external identifier of the content collection.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }
}
