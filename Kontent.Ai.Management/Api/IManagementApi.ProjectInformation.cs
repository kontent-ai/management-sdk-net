using Kontent.Ai.Management.Models.Environments;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Gets information about the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("")]
    internal Task<IApiResponse<EnvironmentInformationModel>> GetEnvironmentInformationInternalAsync(
        CancellationToken cancellationToken = default);
}
