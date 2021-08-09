using System;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Shared
{
    /// <summary>
    /// Represents identifier of the language.
    /// </summary>
    //todo rename this class
    public sealed class NoExternalIdIdentifier
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
        public static NoExternalIdIdentifier ById(Guid id)
        {
            return new NoExternalIdIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static NoExternalIdIdentifier ByCodename(string codename)
        {
            return new NoExternalIdIdentifier() { Codename = codename };
        }
    }
}
