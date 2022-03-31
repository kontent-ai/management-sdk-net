using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow;

/// <summary>
/// Represents the workflow step identifier.
/// </summary>
public class WorkflowStepIdentifier
{
    /// <summary>
    /// Represents the identifier of the workflow.
    /// </summary>
    [JsonProperty("workflow_identifier")]
    public Reference Workflow { get; private set; }

    /// <summary>
    /// Represents the identifier of the step in the workflow.
    /// </summary>
    [JsonProperty("step_identifier")]
    public Reference Step { get; private set; }

    /// <summary>
    /// Creates an instance of the workflow step identifier.
    /// </summary>
    /// <param name="workflowIdentifier">The identifier of the workflow.</param>
    /// <param name="stepIdentifier">The identifier of the workflow step.</param>
    public WorkflowStepIdentifier(Reference workflowIdentifier, Reference stepIdentifier)
    {
        Workflow = workflowIdentifier;
        Step = stepIdentifier;
    }
}
