using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageModel>> ListLanguagesAsync()
    {
        var endpointUrl = _urlBuilder.BuildLanguagesUrl();
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguagesListingResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<LanguageModel>(
            GetNextListingPageAsync<LanguagesListingResponseServerModel, LanguageModel>,
            response.Pagination?.Token,
            endpointUrl,
            response.Languages);
    }

    /// <inheritdoc />
    public async Task<LanguageModel> GetLanguageAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildLanguagesUrl(identifier);
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<LanguageModel>(endpointUrl, HttpMethod.Get);

        return response;
    }

    /// <inheritdoc />
    public async Task<LanguageModel> CreateLanguageAsync(LanguageCreateModel language)
    {
        ArgumentNullException.ThrowIfNull(language);

        var endpointUrl = _urlBuilder.BuildLanguagesUrl();
        return await _actionInvoker.InvokeMethodAsync<LanguageCreateModel, LanguageModel>(endpointUrl, HttpMethod.Post, language);
    }

    /// <inheritdoc />
    public async Task<LanguageModel> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var endpointUrl = _urlBuilder.BuildLanguagesUrl(identifier);
        return await _actionInvoker.InvokeMethodAsync<IEnumerable<LanguagePatchModel>, LanguageModel>(endpointUrl, new HttpMethod("PATCH"), changes);
    }
}
