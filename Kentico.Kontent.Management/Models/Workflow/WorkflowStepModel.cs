using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Workflow
{
    /// <summary>
    /// Represenst the workflow step model.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#section/Workflow-step-object
    /// </summary>
    public class WorkflowStepModel
    {
        /// <summary>
        /// Gets or sets the workflow step's internal ID.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the workflow step's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the workflow step's codename.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets the workflow steps that this step can transition to, represented by their internal IDs.
        /// </summary>
        [JsonProperty("transitions_to")]
        public IEnumerable<Guid> TransitionsTo { get; set; }
    }
}
