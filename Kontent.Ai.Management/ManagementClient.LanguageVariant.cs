using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.ListLanguageVariantsByItemInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<List<LanguageVariantModel<T>>> ListLanguageVariantsByItemAsync<T>(Reference identifier) where T : new()
        => (await ListLanguageVariantsByItemAsync(identifier)).Select(_modelProvider.GetLanguageVariantModel<T>).ToList();

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsByTypePageAsync(typeSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel<T>>> ListLanguageVariantsByTypeAsync<T>(Reference identifier) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment));

        return new ListingResponseMappedModel<LanguageVariantModel, LanguageVariantModel<T>>(
            (continuationToken, _) => GetNextLanguageVariantsByTypePageAsync(typeSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants,
            _modelProvider.GetLanguageVariantModel<T>);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsByTypePageAsync(string typeSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsByTypeInternalAsync(typeSegment, continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var typeSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsOfContentTypeWithComponentsInternalAsync(typeSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsWithComponentsPageAsync(typeSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsWithComponentsPageAsync(string typeSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsOfContentTypeWithComponentsInternalAsync(typeSegment, continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var collectionSegment = identifier.ToUrlSegment();
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsByCollectionInternalAsync(collectionSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsByCollectionPageAsync(collectionSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsByCollectionPageAsync(string collectionSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsByCollectionInternalAsync(collectionSegment, continuationToken));

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsBySpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var spaceSegment = identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename);
        var response = EnsureSuccess(await _managementApi.ListLanguageVariantsBySpaceInternalAsync(spaceSegment));

        return new ListingResponseModel<LanguageVariantModel>(
            (continuationToken, _) => GetNextLanguageVariantsBySpacePageAsync(spaceSegment, continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Variants);
    }

    private async Task<IListingResponse<LanguageVariantModel>> GetNextLanguageVariantsBySpacePageAsync(string spaceSegment, string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguageVariantsBySpaceInternalAsync(spaceSegment, continuationToken));

    /// <inheritdoc />
    public async Task<LanguageVariantModel> GetLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
        => _modelProvider.GetLanguageVariantModel<T>(await GetLanguageVariantAsync(identifier));

    /// <inheritdoc />
    public async Task<LanguageVariantModel> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetPublishedLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> GetPublishedLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
        => _modelProvider.GetLanguageVariantModel<T>(await GetPublishedLanguageVariantAsync(identifier));

    /// <inheritdoc />
    public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariantUpsertModel);

        return EnsureSuccess(await _managementApi.UpsertLanguageVariantInternalAsync(BuildVariantPath(identifier), languageVariantUpsertModel));
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(languageVariant);

        return await UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel(languageVariant));
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> UpsertLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, T variantElements, WorkflowStepIdentifier workflow = null) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(variantElements);

        var variantUpsertModel = _modelProvider.GetLanguageVariantUpsertModel(variantElements, workflow);

        return _modelProvider.GetLanguageVariantModel<T>(await UpsertLanguageVariantAsync(identifier, variantUpsertModel));
    }

    /// <inheritdoc />
    public async Task DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteLanguageVariantInternalAsync(BuildVariantPath(identifier)));
    }

    // A variant is addressed as `{item}/variants/{language}`; the language part supports id or codename only.
    private static string BuildVariantPath(LanguageVariantIdentifier identifier) =>
        $"{identifier.ItemIdentifier.ToUrlSegment()}/variants/{identifier.LanguageIdentifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)}";
}
