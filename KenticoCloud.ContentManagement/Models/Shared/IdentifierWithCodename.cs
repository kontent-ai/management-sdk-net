using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    /// <summary>
    /// Represents the identifier with codename.
    /// </summary>
    public class IdentifierWithCodename<T>: Identifier<T> where T : IdentifierWithCodename<T>, new()
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

        protected IdentifierWithCodename() { }
    }
}
