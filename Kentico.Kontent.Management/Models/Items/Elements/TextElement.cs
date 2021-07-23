using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.Items.Elements
{
    /// <summary>
    /// Represents strongly typed url  slug element
    /// </summary>
    public class TextElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of the url slug.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        public TextElement(dynamic data = null)
        {
            if (data != null)
            {
                Value = data.value?.ToString();
            }
        }

        public override dynamic ToDynamic(string elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
            };
        }
    }
}
