using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Languages.Patch;

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
    public Task<IManagementResult<LanguageModel>> GetLanguageAsync(Reference identifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return _managementApi.GetLanguageInternalAsync(identifier.ToUrlSegment(), cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageModel>> CreateLanguageAsync(LanguageCreateModel language, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(language);

        return _managementApi.CreateLanguageInternalAsync(language, cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<LanguageModel>> ModifyLanguageAsync(Reference identifier, IEnumerable<LanguagePatchModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        return _managementApi.ModifyLanguageInternalAsync(identifier.ToUrlSegment(), changes, cancellationToken).ToManagementResultAsync();
    }
}
