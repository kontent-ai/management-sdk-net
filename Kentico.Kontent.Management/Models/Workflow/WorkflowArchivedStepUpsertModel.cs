using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow
{
    /// <summary>
    /// Represents the Archived workflow step upsert model.
    /// </summary>
    public class WorkflowArchivedStepUpsertModel
    {
        /// <summary>
        /// Gets or sets the roles which can work with an item in this step.
        /// </summary>
        [JsonProperty("role_ids", Required = Required.Always)]
        public IReadOnlyCollection<Guid> RoleIds { get; set; }
    }
}
