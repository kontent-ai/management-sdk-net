using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Languages.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's languages.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/languages")]
    internal Task<IApiResponse<LanguagesListingResponseServerModel>> ListLanguagesInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single language.</summary>
    /// <param name="identifier">The language identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/languages/{**identifier}")]
    internal Task<IApiResponse<LanguageModel>> GetLanguageInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new language.</summary>
    /// <param name="language">The language to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/languages")]
    internal Task<IApiResponse<LanguageModel>> CreateLanguageInternalAsync(
        [Body] LanguageCreateModel language,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to an existing language.</summary>
    /// <param name="identifier">The language identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/languages/{**identifier}")]
    internal Task<IApiResponse<LanguageModel>> ModifyLanguageInternalAsync(
        string identifier,
        [Body] IEnumerable<LanguagePatchModel> changes,
        CancellationToken cancellationToken = default);
}
