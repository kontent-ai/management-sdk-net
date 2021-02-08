using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Represents asset folder identifier.
    /// </summary>
    public sealed class AssetFolderIdentifier
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AssetFolderIdentifier()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000000"); //Default to the Guid that means "outside of any folder"
        }

        /// <summary>
        /// Gets id of the identifier. "00000000-0000-0000-0000-000000000000" means outside of any folder.
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
        public static AssetFolderIdentifier ById(Guid id)
        {
            return new AssetFolderIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by external id.
        /// </summary>
        /// <param name="externalId">The external id of the identifier.</param>
        public static AssetFolderIdentifier ByExternalId(string externalId)
        {
            return new AssetFolderIdentifier() { ExternalId = externalId };
        }
    }
}
