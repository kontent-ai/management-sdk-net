using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.Items.Elements
{
    /// <summary>
    /// Represents strongly typed url  slug element
    /// </summary>
    public class UrlSlugElement : BaseElement
    {
        /// <summary>
        /// Gets or sets mode of the url slug.
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets value of the url slug.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        public UrlSlugElement(dynamic data = null)
        {
            if (data != null)
            {
                Mode = data.mode?.ToString();
                Value = data.value?.ToString();
            }
        }

        public override dynamic ToDynamic(string elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
                mode = Mode
            };
        }
    }
}
