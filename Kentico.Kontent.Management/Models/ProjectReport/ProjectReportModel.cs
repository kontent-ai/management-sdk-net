using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.ProjectReport
{
    /// <summary>
    /// Represents project report model
    /// </summary>
    public sealed class ProjectReportModel
    {
        /// <summary>
        /// Gets or sets information about specified project
        /// </summary>
        [JsonProperty("project")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets reports of the problems found in the project's content
        /// </summary>
        [JsonProperty("variant_issues")]
        public List<VariantIssue> VariantIssues { get; set; }

        /// <summary>
        /// Gets or sets reports of the problems found in the project's content type
        /// </summary>
        [JsonProperty("type_issues")]
        public List<TypeIssues> TypeIssues { get; set; }
    }
}

