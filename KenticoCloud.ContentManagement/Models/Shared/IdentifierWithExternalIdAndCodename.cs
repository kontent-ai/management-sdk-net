using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public class IdentifierWithExternalIdAndCodename<T> : IdentifierWithExternalId<T> where T : IdentifierWithExternalIdAndCodename<T>, new()
    {
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }

        public static T ByCodename(string codename)
        {
            return new T() { Codename = codename };
        }

        protected IdentifierWithExternalIdAndCodename() { }
    }
}
