using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Clones the environment.</summary>
    /// <param name="cloneEnvironmentModel">The clone settings.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Post("/clone-environment")]
    internal Task<IApiResponse<EnvironmentClonedModel>> CloneEnvironmentInternalAsync(
        [Body] EnvironmentCloneModel cloneEnvironmentModel,
        CancellationToken cancellationToken = default);

    /// <summary>Gets the cloning state of the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/environment-cloning-state")]
    internal Task<IApiResponse<EnvironmentCloningStateModel>> GetEnvironmentCloningStateInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Deletes the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Delete("")]
    internal Task<IApiResponse> DeleteEnvironmentInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Marks the environment as production.</summary>
    /// <param name="markAsProductionModel">The mark-as-production settings.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/mark-environment-as-production")]
    internal Task<IApiResponse> MarkEnvironmentAsProductionInternalAsync(
        [Body] MarkAsProductionModel markAsProductionModel,
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to the environment.</summary>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("")]
    internal Task<IApiResponse<EnvironmentModel>> ModifyEnvironmentInternalAsync(
        [Body] IEnumerable<EnvironmentOperationBaseModel> changes,
        CancellationToken cancellationToken = default);
}
