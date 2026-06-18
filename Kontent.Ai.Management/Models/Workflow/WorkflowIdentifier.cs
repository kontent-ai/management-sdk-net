
namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Identifies the content item variant and workflow step used by workflow operations.
/// </summary>
public sealed record WorkflowIdentifier
{
    /// <summary>
    /// The identifier of the content item.
    /// </summary>
    public Reference ItemIdentifier { get; }

    /// <summary>
    /// The identifier of the language variant.
    /// </summary>
    public Reference LanguageIdentifier { get; }

    /// <summary>
    /// The identifier of the workflow step.
    /// </summary>
    public Reference WorkflowStepIdentifier { get; }

    /// <summary>
    /// Creates a workflow-operation identifier from content item, language, and workflow step references.
    /// </summary>
    /// <param name="itemIdentifier">The identifier of the content item.</param>
    /// <param name="languageIdentifier">The identifier of the language.</param>
    /// <param name="stepIdentifier">The identifier of the workflow step.</param>
    public WorkflowIdentifier(Reference itemIdentifier, Reference languageIdentifier, Reference stepIdentifier)
    {
        ItemIdentifier = itemIdentifier;
        LanguageIdentifier = languageIdentifier;
        WorkflowStepIdentifier = stepIdentifier;
    }
}
