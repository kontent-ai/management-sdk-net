using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Collections
{
    /// <summary>
    /// Represents content collections
    /// </summary>
    public class CollectionsModel
    {
        /// <summary>
        /// Gets or sets list of content collections
        /// </summary>
        [JsonProperty("collections")]
        public IEnumerable<CollectionModel> Collections { get; set; }

        /// <summary>
        /// Gets or sets the ISO-8601 formatted date and time of the last change to content collections.
        /// This property can be null if the collections were not changed yet.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
