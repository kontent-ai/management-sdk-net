using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow
{
    /// <summary>
    /// Represents the workflow upsert model.
    /// </summary>
    public class WorkflowUpsertModel
    {
        /// <summary>
        /// Gets or sets the workflow's name.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the workflow's codename.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets the workflow's scopes.
        /// </summary>
        [JsonProperty("scopes", Required = Required.Always)]
        public IReadOnlyList<WorkflowScopeUpsertModel> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the workflow's steps.
        /// </summary>
        [JsonProperty("steps", Required = Required.Always)]
        public IReadOnlyList<WorkflowStepUpsertModel> Steps { get; set; }

        /// <summary>
        /// Gets or sets the workflow's Published step.
        /// </summary>
        [JsonProperty("published_step", Required = Required.Always)]
        public WorkflowPublishedStepUpsertModel PublishedStep { get; set; }

        /// <summary>
        /// Gets or sets the workflow's Archived step.
        /// </summary>
        [JsonProperty("archived_step", Required = Required.Always)]
        public WorkflowArchivedStepUpsertModel ArchivedStep { get; set; }
    }
}
