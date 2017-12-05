using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public class IdentifierWithCodename<T>: Identifier<T> where T : IdentifierWithCodename<T>, new()
    {
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }

        public static T ByCodename(string codename)
        {
            return new T() { Codename = codename };
        }

        protected IdentifierWithCodename() { }
    }
}
