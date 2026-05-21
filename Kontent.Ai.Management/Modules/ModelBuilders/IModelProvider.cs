using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Modules.ModelBuilders;

/// <summary>
/// Defines the contract for mapping content items to strongly typed models.
/// </summary>
public interface IModelProvider
{
    /// <summary>
    /// Builds a strongly typed language variant model from non-generic model.
    /// </summary>
    /// <typeparam name="T">Strongly typed elements model.</typeparam>
    /// <param name="variant">Language variant data.</param>
    /// <returns>Strongly typed language variant model of the generic type.</returns>
    LanguageVariantModel<T> GetLanguageVariantModel<T>(LanguageVariantModel variant) where T : new();

    /// <summary>
    /// Converts generic language variant upsert model to non-generic model.
    /// </summary>
    /// <typeparam name="T">Strongly typed elements model.</typeparam>
    /// <param name="variantElements">Strongly typed language variant elements data.</param>
    /// <param name="workflow">Workflow data</param>
    /// <returns>Non-generic language variant model.</returns>
    LanguageVariantUpsertModel GetLanguageVariantUpsertModel<T>(T variantElements, WorkflowStepIdentifier workflow = null) where T : new();
}
