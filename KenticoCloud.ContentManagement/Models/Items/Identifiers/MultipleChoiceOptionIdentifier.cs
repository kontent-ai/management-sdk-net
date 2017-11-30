using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class MultipleChoiceOptionIdentifier
    {
        [JsonProperty("codename")]
        public string Codename { get; private set; }

        public static MultipleChoiceOptionIdentifier ByCodename(string codename)
        {
            return new MultipleChoiceOptionIdentifier() { Codename = codename };
        }
    }
}
