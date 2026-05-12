using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's content types.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/types")]
    internal Task<IApiResponse<ContentTypeListingResponseServerModel>> ListContentTypesInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single content type.</summary>
    /// <param name="identifier">The content type identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/types/{**identifier}")]
    internal Task<IApiResponse<ContentTypeModel>> GetContentTypeInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new content type.</summary>
    /// <param name="contentType">The content type to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/types")]
    internal Task<IApiResponse<ContentTypeModel>> CreateContentTypeInternalAsync(
        [Body] ContentTypeCreateModel contentType,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a content type.</summary>
    /// <param name="identifier">The content type identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/types/{**identifier}")]
    internal Task<IApiResponse> DeleteContentTypeInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to an existing content type.</summary>
    /// <param name="identifier">The content type identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/types/{**identifier}")]
    internal Task<IApiResponse<ContentTypeModel>> ModifyContentTypeInternalAsync(
        string identifier,
        [Body] IEnumerable<ContentTypeOperationBaseModel> changes,
        CancellationToken cancellationToken = default);
}
