using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Represents the text element that provides the default value to the URL slug element.
    /// The dependent text element can be part of a content type snippet.
    /// </summary>
    public class UrlSlugDependency
    {
        /// <summary>
        /// Gets or sets the content type snippet, specified as a reference, that contains the dependent text element.
        /// Note: The snippet property is not present if the text element is in the same content type.
        /// </summary>
        [JsonProperty("snippet", NullValueHandling = NullValueHandling.Ignore)]
        public ObjectIdentifier SnippetIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the dependent text element specified as a reference.
        /// </summary>
        [JsonProperty("element")]
        public ObjectIdentifier Element { get; set; }
    }
}