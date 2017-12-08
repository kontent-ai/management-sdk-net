using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public class MultipleChoiceOptionIdentifier
    {
        /// <summary>
        /// Gets codename of the identifier.
        /// </summary>
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }


        private MultipleChoiceOptionIdentifier()
        {
        }
        
        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static MultipleChoiceOptionIdentifier ByCodename(string codename)
        {
            return new MultipleChoiceOptionIdentifier() { Codename = codename };
        }
    }
}
