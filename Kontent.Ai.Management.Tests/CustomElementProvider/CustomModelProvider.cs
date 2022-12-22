using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Modules.ModelBuilders;
using System;

namespace Kontent.Ai.Management.Tests.CustomElementProvider;

internal class CustomModelProvider : IModelProvider
{
    private readonly IElementModelProvider _elementModelProvider;

    internal CustomModelProvider(IManagementClient managementClient)
    {
        _elementModelProvider = new CustomElementModelProvider(managementClient);
    }

    public LanguageVariantModel<T> GetLanguageVariantModel<T>(LanguageVariantModel variant) where T : new() => new()
    {
        Item = variant.Item,
        Language = variant.Language,
        LastModified = variant.LastModified,
        Workflow = variant.Workflow,
        Elements = _elementModelProvider.GetStronglyTypedElements<T>(variant.Elements)
    };

    public LanguageVariantUpsertModel GetLanguageVariantUpsertModel<T>(T variantElements, WorkflowStepIdentifier workflow = null) where T : new() => new()
    {
        Elements = _elementModelProvider.GetDynamicElements(variantElements),
        Workflow = workflow,
    };

    public AssetModel<T> GetAssetModel<T>(AssetModel asset) where T : new() => throw new NotImplementedException();

    public AssetCreateModel GetAssetCreateModel<T>(AssetCreateModel<T> asset) where T : new() => throw new NotImplementedException();

    public AssetUpsertModel GetAssetUpsertModel<T>(AssetUpsertModel<T> asset) where T : new() => throw new NotImplementedException();
}
