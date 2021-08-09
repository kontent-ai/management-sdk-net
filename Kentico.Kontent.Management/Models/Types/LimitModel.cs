using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types
{
    public class LimitModel
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("condition")]
        public LimitType Condition { get; set; }
    }
}
