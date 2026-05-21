using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Modules.ModelBuilders;

internal class ModelProvider : IModelProvider
{
    private readonly IElementModelProvider _elementModelProvider;

    internal ModelProvider()
    {
        _elementModelProvider = new ElementModelProvider();
    }

    public LanguageVariantModel<T> GetLanguageVariantModel<T>(LanguageVariantModel variant) where T : new() =>
        new()
        {
            Item = variant.Item,
            Language = variant.Language,
            LastModified = variant.LastModified,
            Schedule = variant.Schedule,
            Workflow = variant.Workflow,
            DueDate = variant.DueDate,
            Note = variant.Note,
            Contributors = variant.Contributors,
            Elements = _elementModelProvider.GetStronglyTypedElements<T>(variant.Elements),
        };

    public LanguageVariantUpsertModel GetLanguageVariantUpsertModel<T>(T variantElements, WorkflowStepIdentifier workflow = null) where T : new() =>
    new()
    {
        Elements = _elementModelProvider.GetDynamicElements(variantElements),
        Workflow = workflow,
    };
}
