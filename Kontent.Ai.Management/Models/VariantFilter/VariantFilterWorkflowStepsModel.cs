using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter workflow steps model.
/// </summary>
public class VariantFilterWorkflowStepsModel
{
    /// <summary>
    /// Gets or sets the workflow reference.
    /// </summary>
    [JsonProperty("workflow_identifier")]
    public Reference WorkflowReference { get; set; }

    /// <summary>
    /// Gets or sets the workflow step references.
    /// </summary>
    [JsonProperty("step_identifiers")]
    public IEnumerable<Reference> WorkflowStepReferences { get; set; }
}