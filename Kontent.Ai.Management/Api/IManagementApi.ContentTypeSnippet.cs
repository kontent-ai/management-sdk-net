using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's content type snippets.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/snippets")]
    internal Task<IApiResponse<SnippetListingResponseServerModel>> ListContentTypeSnippetsInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single content type snippet.</summary>
    /// <param name="identifier">The snippet identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/snippets/{**identifier}")]
    internal Task<IApiResponse<ContentTypeSnippetModel>> GetContentTypeSnippetInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new content type snippet.</summary>
    /// <param name="contentTypeSnippet">The snippet to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/snippets")]
    internal Task<IApiResponse<ContentTypeSnippetModel>> CreateContentTypeSnippetInternalAsync(
        [Body] ContentTypeSnippetCreateModel contentTypeSnippet,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a content type snippet.</summary>
    /// <param name="identifier">The snippet identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/snippets/{**identifier}")]
    internal Task<IApiResponse> DeleteContentTypeSnippetInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to an existing content type snippet.</summary>
    /// <param name="identifier">The snippet identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/snippets/{**identifier}")]
    internal Task<IApiResponse<ContentTypeSnippetModel>> ModifyContentTypeSnippetInternalAsync(
        string identifier,
        [Body] IEnumerable<ContentTypeSnippetOperationBaseModel> changes,
        CancellationToken cancellationToken = default);
}
