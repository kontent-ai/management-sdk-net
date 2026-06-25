using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists one page of the environment's taxonomy groups.</summary>
    /// <param name="continuationToken">Continuation token from a previous page's response; <c>null</c> for the first page.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/taxonomies")]
    internal Task<IApiResponse<TaxonomyGroupListingResponseServerModel>> ListTaxonomyGroupsInternalAsync(
        [Header("x-continuation")] string? continuationToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single taxonomy group.</summary>
    /// <param name="identifier">The taxonomy group identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/taxonomies/{**identifier}")]
    internal Task<IApiResponse<TaxonomyGroupModel>> GetTaxonomyGroupInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new taxonomy group.</summary>
    /// <param name="taxonomyGroup">The taxonomy group to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/taxonomies")]
    internal Task<IApiResponse<TaxonomyGroupModel>> CreateTaxonomyGroupInternalAsync(
        [Body] TaxonomyGroupCreateModel taxonomyGroup,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to an existing taxonomy group.</summary>
    /// <param name="identifier">The taxonomy group identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/taxonomies/{**identifier}")]
    internal Task<IApiResponse<TaxonomyGroupModel>> ModifyTaxonomyGroupInternalAsync(
        string identifier,
        [Body] IEnumerable<TaxonomyGroupOperationBaseModel> changes,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a taxonomy group.</summary>
    /// <param name="identifier">The taxonomy group identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment(Reference)"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/taxonomies/{**identifier}")]
    internal Task<IApiResponse> DeleteTaxonomyGroupInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}
