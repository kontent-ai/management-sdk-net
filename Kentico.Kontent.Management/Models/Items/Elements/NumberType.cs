using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Models.Items.Elements
{
    /// <summary>
    /// Represents strongly typed url  slug element
    /// </summary>
    public class NumberElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of the url slug.
        /// </summary>
        [JsonProperty("value")]
        public decimal? Value { get; set; }

        public NumberElement(JToken data = null) : base(data)
        {
            if (data != null)
            {
                Value = Convert.ToDecimal(data["value"]);
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
