using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Models.Workflow
{
    public class WorkflowIdentifier
    {
        /// <summary>
        /// Represents identifier of the content item variant.
        /// </summary>
        public Reference ItemIdentifier { get; }

        /// <summary>
        /// Represents identifier of the language.
        /// </summary>
        public NoExternalIdIdentifier LanguageIdentifier { get; }

        /// <summary>
        /// Represents identifier of the workflow step.
        /// </summary>
        public NoExternalIdIdentifier WorkflowStepIdentifier { get; }

        /// <summary>
        /// Creates instance of the workflow step identifier.
        /// </summary>
        /// <param name="itemIdentifier">The identifier of the content item.</param>
        /// <param name="languageIdentifier">The identifier of the language.</param>
        /// /// <param name="stepIdentifier">The identifier of the workflow step.</param>
        public WorkflowIdentifier(Reference itemIdentifier, NoExternalIdIdentifier languageIdentifier, NoExternalIdIdentifier stepIdentifier)
        {
            ItemIdentifier = itemIdentifier;
            LanguageIdentifier = languageIdentifier;
            WorkflowStepIdentifier = stepIdentifier;
        }
    }
}
