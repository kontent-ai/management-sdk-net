using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups
{
    /// <summary>
    /// Represents the taxonomy group model.
    /// </summary>
    public class TaxonomyGroupModel : TaxonomyBaseModel
    {
        /// <summary>
        /// Gets or sets ISO-8601 formatted date/time of the last change to the taxonomy group or its terms.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
