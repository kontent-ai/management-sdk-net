using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow
{
    /// <summary>
    /// Represents the custom workflow step upsert model.
    /// </summary>
    public class WorkflowStepUpsertModel
    {
        /// <summary>
        /// Gets or sets the workflow step's identifier. 
        /// </summary>
        /// <remarks>
        /// Not applicable for creating a new workflow.
        /// </remarks>
        [JsonProperty("id", Required = Required.Default)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the workflow step's name.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the workflow step's codename.
        /// </summary>
        [JsonProperty("codename", Required = Required.Always)]
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets the workflow step's color.
        /// </summary>
        [JsonProperty("color", Required = Required.Always)]
        public WorkflowStepColorModel Color { get; set; }

        /// <summary>
        /// Gets or sets the workflow steps that this step can transition to.
        /// </summary>
        [JsonProperty("transitions_to", Required = Required.Always)]
        public IReadOnlyList<WorkflowStepTransitionToUpsertModel> TransitionsTo { get; set; }

        /// <summary>
        /// Gets or sets the roles which can work with an item in this step.
        /// </summary>
        [JsonProperty("role_ids", Required = Required.Always)]
        public IReadOnlyCollection<Guid> RoleIds { get; set; }
    }
}
