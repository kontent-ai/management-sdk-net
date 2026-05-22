using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter workflow steps model.
/// </summary>
public sealed record VariantFilterWorkflowStepsModel
{
    /// <summary>
    /// Gets the workflow reference.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public Reference WorkflowReference { get; init; }

    /// <summary>
    /// Gets the workflow step references.
    /// </summary>
    [JsonPropertyName("step_identifiers")]
    public IEnumerable<Reference> WorkflowStepReferences { get; init; }
}