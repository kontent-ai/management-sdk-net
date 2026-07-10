using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Converts fetched models into the <see cref="Reference"/> other calls expect, so an object you already hold can be
/// fed back into the client without repeating <c>Reference.ById(model.Id)</c>.
/// </summary>
public static class ModelReferenceExtensions
{
    /// <summary>Creates a <see cref="Reference"/> to this content item by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this ContentItemModel item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return Reference.ById(item.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this asset by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by external id (<see cref="Reference.ByExternalId"/>) instead.</remarks>
    public static Reference ToReference(this AssetModel asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        return Reference.ById(asset.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this content type by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this ContentTypeModel contentType)
    {
        ArgumentNullException.ThrowIfNull(contentType);
        return Reference.ById(contentType.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this content type snippet by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this ContentTypeSnippetModel snippet)
    {
        ArgumentNullException.ThrowIfNull(snippet);
        return Reference.ById(snippet.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this taxonomy group by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this TaxonomyGroupModel taxonomyGroup)
    {
        ArgumentNullException.ThrowIfNull(taxonomyGroup);
        return Reference.ById(taxonomyGroup.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this language by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this LanguageModel language)
    {
        ArgumentNullException.ThrowIfNull(language);
        return Reference.ById(language.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this workflow by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this WorkflowModel workflow)
    {
        ArgumentNullException.ThrowIfNull(workflow);
        return Reference.ById(workflow.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this collection by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this CollectionModel collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return Reference.ById(collection.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this space by its id.</summary>
    /// <remarks>Ids are environment-specific; scripts targeting a different environment should reference by codename (<see cref="Reference.ByCodename"/>) instead.</remarks>
    public static Reference ToReference(this SpaceModel space)
    {
        ArgumentNullException.ThrowIfNull(space);
        return Reference.ById(space.Id);
    }

    /// <summary>Creates a <see cref="Reference"/> to this webhook by its id.</summary>
    /// <remarks>Ids are environment-specific; a webhook has no portable identifier, so cross-environment scripts must locate it by other means (e.g. its name).</remarks>
    public static Reference ToReference(this WebhookModel webhook)
    {
        ArgumentNullException.ThrowIfNull(webhook);
        return Reference.ById(webhook.Id);
    }
}
