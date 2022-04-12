using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow
{
    /// <summary>
    /// Represents the workflow step's 'transition to' upsert model.
    /// </summary>
    public class WorkflowStepTransitionToUpsertModel
    {
        /// <summary>
        /// Gets or sets the workflow step's internal ID.
        /// </summary>
        [JsonProperty("step", Required = Required.Always)]
        public Reference Step { get; set; }
    }
}
