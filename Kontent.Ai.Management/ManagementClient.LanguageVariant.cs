using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IEnumerable<LanguageVariantModel>> ListLanguageVariantsByItemAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsByItemUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<IEnumerable<LanguageVariantModel>>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByTypeAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsByTypeUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<LanguageVariantModel>(
                GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel<T>>> ListLanguageVariantsByTypeAsync<T>(Reference identifier) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsByTypeUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseMappedModel<LanguageVariantModel, LanguageVariantModel<T>>(
                GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Variants,
                _modelProvider.GetLanguageVariantModel<T>);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsOfContentTypeWithComponentsAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsByComponentUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<LanguageVariantModel>(
                GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsByCollectionAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsByCollectionUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<LanguageVariantModel>(
                GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Variants);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageVariantModel>> ListLanguageVariantsBySpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsBySpaceUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantsListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<LanguageVariantModel>(
            GetNextListingPageAsync<LanguageVariantsListingResponseServerModel, LanguageVariantModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Variants);
    }

    /// <inheritdoc />
    public async Task<List<LanguageVariantModel<T>>> ListLanguageVariantsByItemAsync<T>(Reference identifier) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildListVariantsByItemUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<List<LanguageVariantModel>>(endpointUrl, HttpMethod.Get);

        return response.Select(_modelProvider.GetLanguageVariantModel<T>).ToList();
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> GetLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> GetLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

        return _modelProvider.GetLanguageVariantModel<T>(response);
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> GetPublishedLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildPublishedVariantsUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> GetPublishedLanguageVariantAsync<T>(LanguageVariantIdentifier identifier) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildPublishedVariantsUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageVariantModel>(endpointUrl, HttpMethod.Get);

        return _modelProvider.GetLanguageVariantModel<T>(response);
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantUpsertModel languageVariantUpsertModel)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(languageVariantUpsertModel);

        var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
        var response = await _actionInvoker.InvokeMethodAsync<LanguageVariantUpsertModel, LanguageVariantModel>(endpointUrl, HttpMethod.Put, languageVariantUpsertModel);

        return response;
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel> UpsertLanguageVariantAsync(LanguageVariantIdentifier identifier, LanguageVariantModel languageVariant)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        ArgumentNullException.ThrowIfNull(languageVariant);

        var languageVariantUpsertModel = new LanguageVariantUpsertModel(languageVariant);

        return await UpsertLanguageVariantAsync(identifier, languageVariantUpsertModel);
    }

    /// <inheritdoc />
    public async Task<LanguageVariantModel<T>> UpsertLanguageVariantAsync<T>(LanguageVariantIdentifier identifier, T variantElements, WorkflowStepIdentifier workflow = null) where T : new()
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (variantElements == null)
        {
            throw new ArgumentNullException(nameof(variantElements));
        }

        var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
        var variantUpsertModel = _modelProvider.GetLanguageVariantUpsertModel(variantElements, workflow);
        var response = await _actionInvoker.InvokeMethodAsync<LanguageVariantUpsertModel, LanguageVariantModel>(endpointUrl, HttpMethod.Put, variantUpsertModel);

        return _modelProvider.GetLanguageVariantModel<T>(response);
    }

    /// <inheritdoc />
    public async Task DeleteLanguageVariantAsync(LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildVariantsUrl(identifier);
        await _actionInvoker.InvokeMethodAsync(endpointUrl, HttpMethod.Delete);
    }
}
