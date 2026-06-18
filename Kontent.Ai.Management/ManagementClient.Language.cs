using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Languages;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<IReadOnlyList<LanguageModel>>> ListLanguagesAsync(CancellationToken cancellationToken = default)
        => PageEnumerator.CollectAsync<LanguagesListingResponseServerModel, LanguageModel>(
            _managementApi.ListLanguagesInternalAsync,
            page => page.Languages,
            page => page.Pagination?.Token,
            cancellationToken);

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageModel>> GetLanguageAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.GetLanguageInternalAsync(identifier.ToUrlSegment(), cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageModel>> CreateLanguageAsync(LanguageCreateModel language, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(language);

        var response = await _managementApi.CreateLanguageInternalAsync(language, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<LanguageModel>> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        var response = await _managementApi.ModifyLanguageInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
