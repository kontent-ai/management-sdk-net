using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of the language.
    /// </summary>
    public sealed class LanguageIdentifier
    {
        /// <summary>
        /// Gets id of the identifier.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? Id { get; private set; }

        /// <summary>
        /// Gets codename of the identifier.
        /// </summary>
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }

        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static LanguageIdentifier ById(Guid id)
        {
            return new LanguageIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static LanguageIdentifier ByCodename(string codename)
        {
            return new LanguageIdentifier() { Codename = codename };
        }
        
        /// <summary>
        /// Identifier for default language.
        /// </summary>
        public static readonly LanguageIdentifier DEFAULT_LANGUAGE = ById(Guid.Empty);
    }
}
