using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Lists the content collections in the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/collections")]
    internal Task<IApiResponse<CollectionsModel>> GetCollectionsInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Applies a set of operations to the environment's content collections.</summary>
    /// <param name="changes">The operations to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Patch("/collections")]
    internal Task<IApiResponse<CollectionsModel>> ModifyCollectionsInternalAsync(
        [Body] IEnumerable<CollectionOperationBaseModel> changes,
        CancellationToken cancellationToken = default);
}
