using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups
{
    /// <summary>
    /// Represents the taxonomy group model.
    /// </summary>
    public class TaxonomyGroupModel : TaxonomyBaseModel
    {
        /// <summary>
        /// Gets or sets the taxonomy group's internal ID.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets ISO-8601 formatted date/time of the last change to the taxonomy group or its terms.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets terms in the taxonomy group.
        /// </summary>
        [JsonProperty("terms")]
        public IEnumerable<TaxonomyTermModel> Terms { get; set; }
    }
}
