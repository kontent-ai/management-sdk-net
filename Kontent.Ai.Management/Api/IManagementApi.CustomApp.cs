using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's custom apps.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/custom-apps")]
    internal Task<IApiResponse<CustomAppListingResponseServerModel>> ListCustomAppsInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single custom app.</summary>
    /// <param name="identifier">The custom app identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/custom-apps/{**identifier}")]
    internal Task<IApiResponse<CustomAppModel>> GetCustomAppInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new custom app.</summary>
    /// <param name="customApp">The custom app to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/custom-apps")]
    internal Task<IApiResponse<CustomAppModel>> CreateCustomAppInternalAsync(
        [Body] CustomAppCreateModel customApp,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a custom app.</summary>
    /// <param name="identifier">The custom app identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/custom-apps/{**identifier}")]
    internal Task<IApiResponse> DeleteCustomAppInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to an existing custom app.</summary>
    /// <param name="identifier">The custom app identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/custom-apps/{**identifier}")]
    internal Task<IApiResponse<CustomAppModel>> ModifyCustomAppInternalAsync(
        string identifier,
        [Body] IEnumerable<CustomAppOperationBaseModel> changes,
        CancellationToken cancellationToken = default);
}
