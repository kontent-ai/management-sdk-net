using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public class MaximumTextLengthModel
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("applies_to")]
        public TextLengthLimitType AppliesTo { get; set; }
    }
}