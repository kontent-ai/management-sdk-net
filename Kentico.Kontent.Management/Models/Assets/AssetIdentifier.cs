using System;

using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents asset identifier.
    /// </summary>
    public sealed class AssetIdentifier
    {
        /// <summary>
        /// Gets id of the identifier.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? Id { get; private set; }
        
        /// <summary>
        /// Gets external id of the identifier.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; private set; }

        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static AssetIdentifier ById(Guid id)
        {
            return new AssetIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by external id.
        /// </summary>
        /// <param name="externalId">The external id of the identifier.</param>
        public static AssetIdentifier ByExternalId(string externalId)
        {
            return new AssetIdentifier() { ExternalId = externalId };
        }        
    }
}
