using System;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Items
{
    /// <summary>
    /// Represents identifier of the collection.
    /// </summary>
    public sealed class CollectionIdentifier
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
        public static CollectionIdentifier ById(Guid id)
        {
            return new CollectionIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static CollectionIdentifier ByCodename(string codename)
        {
            return new CollectionIdentifier() { Codename = codename };
        }

        /// <summary>
        /// Identifier for default collection.
        /// </summary>
        public static readonly CollectionIdentifier DEFAULT_COLLECTION = ById(Guid.Empty);
    }
}