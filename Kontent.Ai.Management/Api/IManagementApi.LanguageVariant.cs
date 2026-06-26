using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists all language variants of a content item.</summary>
    /// <param name="itemIdentifier">The content item identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/items/{**itemIdentifier}/variants")]
    internal Task<IApiResponse<IReadOnlyList<LanguageVariantModel>>> ListLanguageVariantsByItemInternalAsync(
        string itemIdentifier,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of the language variants of a content type.</summary>
    /// <param name="typeIdentifier">The content type identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/types/{**typeIdentifier}/variants")]
    internal Task<IApiResponse<LanguageVariantsListingResponseServerModel>> ListLanguageVariantsByTypeInternalAsync(
        string typeIdentifier,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of the language variants that use a content type as a component.</summary>
    /// <param name="typeIdentifier">The content type identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/types/{**typeIdentifier}/components")]
    internal Task<IApiResponse<LanguageVariantsListingResponseServerModel>> ListLanguageVariantsOfContentTypeWithComponentsInternalAsync(
        string typeIdentifier,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of the language variants in a collection.</summary>
    /// <param name="collectionIdentifier">The collection identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/collections/{**collectionIdentifier}/variants")]
    internal Task<IApiResponse<LanguageVariantsListingResponseServerModel>> ListLanguageVariantsByCollectionInternalAsync(
        string collectionIdentifier,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Lists one page of the language variants in a space.</summary>
    /// <param name="spaceIdentifier">The space identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/spaces/{**spaceIdentifier}/variants")]
    internal Task<IApiResponse<LanguageVariantsListingResponseServerModel>> ListLanguageVariantsBySpaceInternalAsync(
        string spaceIdentifier,
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single language variant.</summary>
    /// <param name="variantPath">The <c>{item}/variants/{language}</c> path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(LanguageVariantIdentifier)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/items/{**variantPath}")]
    internal Task<IApiResponse<LanguageVariantModel>> GetLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);

    /// <summary>Gets the published version of a language variant.</summary>
    /// <param name="variantPath">The <c>{item}/variants/{language}</c> path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(LanguageVariantIdentifier)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/items/{**variantPath}/published")]
    internal Task<IApiResponse<LanguageVariantModel>> GetPublishedLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);

    /// <summary>Creates or updates a language variant.</summary>
    /// <param name="variantPath">The <c>{item}/variants/{language}</c> path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(LanguageVariantIdentifier)"/>).</param>
    /// <param name="languageVariantUpsertModel">The variant to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/items/{**variantPath}")]
    internal Task<IApiResponse<LanguageVariantModel>> UpsertLanguageVariantInternalAsync(
        string variantPath,
        [Body] LanguageVariantUpsertModel languageVariantUpsertModel,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a language variant.</summary>
    /// <param name="variantPath">The <c>{item}/variants/{language}</c> path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(LanguageVariantIdentifier)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/items/{**variantPath}")]
    internal Task<IApiResponse> DeleteLanguageVariantInternalAsync(
        string variantPath,
        CancellationToken cancellationToken = default);
}
