using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<LanguageModel>> ListLanguagesAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListLanguagesInternalAsync());

        return new ListingResponseModel<LanguageModel>(
            (continuationToken, _) => GetNextLanguagesPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Languages);
    }

    private async Task<IListingResponse<LanguageModel>> GetNextLanguagesPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListLanguagesInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<LanguageModel> GetLanguageAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetLanguageInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<LanguageModel> CreateLanguageAsync(LanguageCreateModel language)
    {
        ArgumentNullException.ThrowIfNull(language);

        return EnsureSuccess(await _managementApi.CreateLanguageInternalAsync(language));
    }

    /// <inheritdoc />
    public async Task<LanguageModel> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.ModifyLanguageInternalAsync(identifier.ToUrlSegment(), changes));
    }
}
