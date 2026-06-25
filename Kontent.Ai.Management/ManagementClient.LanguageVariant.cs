using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Workflow;
using System.Text.Json;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    // Re-shapes raw element envelopes (a fetched variant's elements, or the envelope converter's output) into the
    // DynamicElement carrier that LanguageVariantUpsertModel.Elements expects. Default options carry the Reference
    // converter the element references need; this internal re-shaping is independent of user-supplied Refit customizations.
    private static readonly JsonSerializerOptions _elementSerializerOptions = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

    private static IReadOnlyList<BaseElement> ToDynamicElements(IEnumerable<object> rawElements)
        => rawElements
            .Select(element => ((JsonElement)element).Deserialize<DynamicElement>(_elementSerializerOptions)!)
            .ToList();
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByItemAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.ListLanguageVariantsByItemInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync<IEnumerable<LanguageVariantModel>, IReadOnlyList<LanguageVariantModel>>(variants => variants.ToList());
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByTypeAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        return PageEnumerator.CollectAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(
            (token, ct) => _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment, token, ct),
            page => page.Variants,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        return PageEnumerator.CollectAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(
            (token, ct) => _managementApi.ListLanguageVariantsOfContentTypeWithComponentsInternalAsync(typeSegment, token, ct),
            page => page.Variants,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByCollectionAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var collectionSegment = identifier.ToUrlSegment();
        return PageEnumerator.CollectAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(
            (token, ct) => _managementApi.ListLanguageVariantsByCollectionInternalAsync(collectionSegment, token, ct),
            page => page.Variants,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsBySpaceAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var spaceSegment = identifier.ToUrlSegment();
        return PageEnumerator.CollectAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(
            (token, ct) => _managementApi.ListLanguageVariantsBySpaceInternalAsync(spaceSegment, token, ct),
            page => page.Variants,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageVariantModel>> GetLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.GetLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageVariantModel<T>>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
        where T : IElementsModel, new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.GetLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync(ToTypedVariant<T>);
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageVariantModel>> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.GetPublishedLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageVariantModel>> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariantUpsertModel);

        return _managementApi.UpsertLanguageVariantInternalAsync(identifier.ToUrlSegment(), languageVariantUpsertModel, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageVariantModel>> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariant);

        var upsertModel = new LanguageVariantUpsertModel
        {
            Elements = ToDynamicElements(languageVariant.Elements),
            Workflow = languageVariant.Workflow,
            DueDate = languageVariant.DueDate,
            Note = languageVariant.Note,
            Contributors = languageVariant.Contributors,
        };

        return await UpsertLanguageVariantAsync(identifier, upsertModel, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageVariantModel<T>>> UpsertLanguageVariantAsync<T>(
        LanguageVariantIdentifier identifier,
        T variant,
        WorkflowStepIdentifier? workflow = null,
        CancellationToken cancellationToken = default)
        where T : IElementsModel, new()
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(variant);

        var upsertModel = new LanguageVariantUpsertModel
        {
            Elements = JsonSerializer.Deserialize<List<DynamicElement>>(_contentConverter.WriteEnvelopes(variant), _elementSerializerOptions)!,
            Workflow = workflow,
        };

        return _managementApi.UpsertLanguageVariantInternalAsync(identifier.ToUrlSegment(), upsertModel, cancellationToken).ToManagementResultAsync(ToTypedVariant<T>);
    }

    /// <inheritdoc />
    public Task<IManagementResult> DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.DeleteLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    // Projects a fetched variant onto the typed wrapper: raw elements become the generated record, the variant
    // metadata (item, language, workflow, …) that the response carries is preserved rather than discarded.
    private LanguageVariantModel<T> ToTypedVariant<T>(LanguageVariantModel variant) where T : IElementsModel, new()
        => new()
        {
            Item = variant.Item,
            Elements = ProjectElements<T>(variant.Elements),
            Language = variant.Language,
            LastModified = variant.LastModified,
            Schedule = variant.Schedule,
            Workflow = variant.Workflow,
            DueDate = variant.DueDate,
            Note = variant.Note,
            Contributors = variant.Contributors,
        };

    // Projects a variant's raw element envelopes into a generated record via the content converter.
    private T ProjectElements<T>(IEnumerable<object>? elements) where T : IElementsModel, new()
    {
        if (_autoScanContentTypes)
        {
            _contentConverter.Registry.Scan(typeof(T).Assembly);
        }

        return _contentConverter.ReadEnvelopes<T>((elements ?? []).Cast<JsonElement>());
    }
}
