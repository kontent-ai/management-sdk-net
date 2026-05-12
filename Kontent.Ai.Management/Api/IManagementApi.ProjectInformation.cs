using Environment = Kontent.Ai.Management.Models.EnvironmentReport.Environment;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Gets information about the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("")]
    internal Task<IApiResponse<Environment>> GetEnvironmentInformationInternalAsync(
        CancellationToken cancellationToken = default);
}
