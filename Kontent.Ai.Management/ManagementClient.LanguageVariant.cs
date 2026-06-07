using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Validation;
using System.Text.Json;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
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

        return await UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel(languageVariant), cancellationToken);
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
            Elements = JsonSerializer.Deserialize<List<object>>(_contentConverter.WriteEnvelopes(variant))!,
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
