using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists all spaces in the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/spaces")]
    internal Task<IApiResponse<IEnumerable<SpaceModel>>> ListSpacesInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Gets a single space.</summary>
    /// <param name="identifier">The space identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/spaces/{**identifier}")]
    internal Task<IApiResponse<SpaceModel>> GetSpaceInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a new space.</summary>
    /// <param name="space">The space to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/spaces")]
    internal Task<IApiResponse<SpaceModel>> CreateSpaceInternalAsync(
        [Body] SpaceCreateModel space,
        CancellationToken cancellationToken = default);

    /// <summary>Replaces properties of an existing space.</summary>
    /// <param name="identifier">The space identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="changes">The replace operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/spaces/{**identifier}")]
    internal Task<IApiResponse<SpaceModel>> ModifySpaceInternalAsync(
        string identifier,
        [Body] IEnumerable<SpaceOperationReplaceModel> changes,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a space.</summary>
    /// <param name="identifier">The space identifier path segment (see <see cref="ReferenceUrlExtensions.ToUrlSegment"/>).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("/spaces/{**identifier}")]
    internal Task<IApiResponse> DeleteSpaceInternalAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}
