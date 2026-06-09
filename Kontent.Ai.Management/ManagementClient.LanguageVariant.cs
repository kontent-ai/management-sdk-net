using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Validation;
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
            .Select(element => JsonSerializer.Deserialize<DynamicElement>(JsonSerializer.Serialize(element, _elementSerializerOptions), _elementSerializerOptions)!)
            .ToList();
    /// <inheritdoc />
    public async Task<IManagementResult<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByItemAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.ListLanguageVariantsByItemInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync<IEnumerable<LanguageVariantModel>, IReadOnlyList<LanguageVariantModel>>(variants => variants.ToList());
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

        var spaceSegment = identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename);
        return PageEnumerator.CollectAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>(
            (token, ct) => _managementApi.ListLanguageVariantsBySpaceInternalAsync(spaceSegment, token, ct),
            page => page.Variants,
            page => page.Pagination?.Token,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageVariantModel>> GetLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
        where T : IContentItem, new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync(content => ProjectElements<T>(content.Elements));
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageVariantModel>> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetPublishedLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageVariantModel>> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariantUpsertModel);

        var response = await _managementApi.UpsertLanguageVariantInternalAsync(identifier.ToUrlSegment(), languageVariantUpsertModel, cancellationToken);
        return await response.ToManagementResultAsync();
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

        return await UpsertLanguageVariantAsync(identifier, upsertModel, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<T>> UpsertLanguageVariantAsync<T>(
        LanguageVariantIdentifier identifier,
        T variant,
        WorkflowStepIdentifier? workflow = null,
        CancellationToken cancellationToken = default)
        where T : IContentItem, new()
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(variant);

        var validation = ContentItemValidator.Validate(variant);
        if (!validation.IsSuccess)
        {
            return validation;
        }

        var upsertModel = new LanguageVariantUpsertModel
        {
            Elements = JsonSerializer.Deserialize<List<DynamicElement>>(_contentConverter.WriteEnvelopes(variant), _elementSerializerOptions)!,
            Workflow = workflow,
        };

        var response = await _managementApi.UpsertLanguageVariantInternalAsync(identifier.ToUrlSegment(), upsertModel, cancellationToken);
        return await response.ToManagementResultAsync(content => ProjectElements<T>(content.Elements));
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.DeleteLanguageVariantInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    // Projects a variant's raw element envelopes into a generated record via the content converter.
    private T ProjectElements<T>(IEnumerable<dynamic> elements) where T : IContentItem, new()
    {
        if (_autoScanContentTypes)
        {
            _contentConverter.Registry.Scan(typeof(T).Assembly);
        }

        var json = JsonSerializer.Serialize(elements ?? []);
        return _contentConverter.ReadEnvelopes<T>(json);
    }
}
