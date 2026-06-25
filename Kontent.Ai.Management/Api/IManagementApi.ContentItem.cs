using Kontent.Ai.Management.Models.Items;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's content items.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/items")]
    internal Task<IApiResponse<ContentItemListingResponseServerModel>> ListContentItemsInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single content item.</summary>
    /// <param name="identifier">The content item identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/items/{**identifier}")]
    internal Task<IApiResponse<ContentItemModel>> GetContentItemInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new content item.</summary>
    /// <param name="contentItem">The content item to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/items")]
    internal Task<IApiResponse<ContentItemModel>> CreateContentItemInternalAsync(
        [Body] ContentItemCreateModel contentItem,
        CancellationToken cancellationToken = default);

    /// <summary>Creates or updates a content item.</summary>
    /// <param name="identifier">The content item identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="contentItem">The content item to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**identifier}")]
    internal Task<IApiResponse<ContentItemModel>> UpsertContentItemInternalAsync(
        string identifier,
        [Body] ContentItemUpsertModel contentItem,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a content item.</summary>
    /// <param name="identifier">The content item identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference, ReferenceKinds)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/items/{**identifier}")]
    internal Task<IApiResponse> DeleteContentItemInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}
