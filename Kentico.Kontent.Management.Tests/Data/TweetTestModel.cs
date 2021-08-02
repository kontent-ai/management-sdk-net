using Kentico.Kontent.Management.Models.Items.Elements;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Tests.Data
{

    public partial class TweetTestModel
    {
        [JsonProperty("tweet_title")]
        [KontentElementId("f09fb430-2a58-59cf-be03-621e1c367501")]
        public TextElement TweetLink { get; set; }

        [JsonProperty("theme")]
        [KontentElementId("05017deb-18b2-5094-b367-e7f8796dd1b8")]
        public MultipleChoiceElement Theme { get; set; }

        [JsonProperty("display_options")]
        [KontentElementId("19e38194-a3a8-5d17-abed-9b70d7a5fd25")]
        public MultipleChoiceElement DisplayOptions { get; set; }
    }
}
