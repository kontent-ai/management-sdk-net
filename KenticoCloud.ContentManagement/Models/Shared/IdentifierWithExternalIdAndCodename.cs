using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    /// <summary>
    /// Represents the identifier with external id and codename.
    /// </summary>
    public class IdentifierWithExternalIdAndCodename<T> : IdentifierWithExternalId<T> where T : IdentifierWithExternalIdAndCodename<T>, new()
    {
        /// <summary>
        /// Gets codename of the identifier.
        /// </summary>
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static T ByCodename(string codename)
        {
            return new T() { Codename = codename };
        }

        protected IdentifierWithExternalIdAndCodename() { }
    }
}
