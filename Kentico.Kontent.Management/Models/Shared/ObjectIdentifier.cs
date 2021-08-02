using System;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models
{
    /// <summary>
    /// Represents the identifier of the api object.
    /// </summary>
    public sealed class ObjectIdentifier
    {
        /// <summary>
        /// Gets or sets id of the reference.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static ObjectIdentifier ById(Guid id)
        {
            return new ObjectIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static ObjectIdentifier ById(string id)
        {
            return new ObjectIdentifier() { Id = Guid.Parse(id) };
        }
    }
}
