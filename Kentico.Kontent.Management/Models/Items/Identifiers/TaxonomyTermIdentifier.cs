using System;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Items
{
    /// <summary>
    /// Represents identifier of the taxonomy term.
    /// </summary>
    public sealed class TaxonomyTermIdentifier
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

        private TaxonomyTermIdentifier()
        {
        }

        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static TaxonomyTermIdentifier ById(Guid id)
        {
            return new TaxonomyTermIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static TaxonomyTermIdentifier ByCodename(string codename)
        {
            return new TaxonomyTermIdentifier() { Codename = codename };
        }
    }
}
