using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    /// <summary>
    /// Represents the identifier with external id.
    /// </summary>
    public abstract class IdentifierWithExternalId<T> : Identifier<T>
        where T : IdentifierWithExternalId<T>, new()
    {
        /// <summary>
        /// Gets external id of the identifier.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; private set; }

        /// <summary>
        /// Creates identifier by external id.
        /// </summary>
        /// <param name="externalId">The external id of the identifier.</param>
        public static T ByExternalId(string externalId)
        {
            return new T() { ExternalId = externalId };
        }

        protected IdentifierWithExternalId() { }
    }
}
