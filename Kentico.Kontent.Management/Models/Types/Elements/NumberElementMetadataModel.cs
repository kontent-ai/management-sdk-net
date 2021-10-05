using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Represents a number element in content types.
    /// </summary>
    public class NumberElementMetadataModel : ElementMetadataBase
    {
        /// <summary>
        /// Gets or sets the element's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a flag determining whether the element must be filled in.
        /// </summary>
        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the element's guidelines, providing instructions on what to fill in.
        /// </summary>
        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }

        /// <summary>
        /// Represents the type of the content type element.
        /// </summary>
        public override ElementMetadataType Type => ElementMetadataType.Number;
    }
}
