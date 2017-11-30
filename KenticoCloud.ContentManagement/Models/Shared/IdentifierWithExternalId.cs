using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public class IdentifierWithExternalId<T> : Identifier<T>
        where T : IdentifierWithExternalId<T>, new()
    {
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; private set; }

        public static T ByExternalId(string externalId)
        {
            return new T() { ExternalId = externalId };
        }

        protected IdentifierWithExternalId() { }
    }
}
