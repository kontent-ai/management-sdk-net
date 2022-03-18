using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Models.Workflow;

/// <summary>
/// Represents the workflow step identifier.
/// </summary>
public class WorkflowIdentifier
{
    /// <summary>
    /// Represents the identifier of the content item variant.
    /// </summary>
    public Reference ItemIdentifier { get; }

    /// <summary>
    /// Represents the identifier of the language.
    /// </summary>
    public Reference LanguageIdentifier { get; }

    /// <summary>
    /// Represents the identifier of the workflow step.
    /// </summary>
    public Reference WorkflowStepIdentifier { get; }

    /// <summary>
    /// Creates an instance of the workflow step identifier.
    /// </summary>
    /// <param name="itemIdentifier">The identifier of the content item.</param>
    /// <param name="languageIdentifier">The identifier of the language.</param>
    /// /// <param name="stepIdentifier">The identifier of the workflow step.</param>
    public WorkflowIdentifier(Reference itemIdentifier, Reference languageIdentifier, Reference stepIdentifier)
    {
        ItemIdentifier = itemIdentifier;
        LanguageIdentifier = languageIdentifier;
        WorkflowStepIdentifier = stepIdentifier;
    }
}
