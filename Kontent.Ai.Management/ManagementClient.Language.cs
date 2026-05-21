using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public IAsyncEnumerable<IManagementResult<IReadOnlyList<LanguageModel>>> EnumerateLanguagePagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.EnumerateAsync<LanguagesListingResponseServerModel, LanguageModel>(
            _managementApi.ListLanguagesInternalAsync,
            page => page.Languages,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageModel>> GetLanguageAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetLanguageInternalAsync(identifier.ToUrlSegment(), cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageModel>> CreateLanguageAsync(LanguageCreateModel language, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(language);

        var response = await _managementApi.CreateLanguageInternalAsync(language, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageModel>> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.ModifyLanguageInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
