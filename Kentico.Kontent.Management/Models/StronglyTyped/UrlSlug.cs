using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.StronglyTyped
{
    /// <summary>
    ///
    /// </summary>
    public class UrlSlug
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
