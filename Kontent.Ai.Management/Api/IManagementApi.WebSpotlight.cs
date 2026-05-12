using Kontent.Ai.Management.Models.WebSpotlight;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Gets the Web Spotlight status of the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/web-spotlight")]
    internal Task<IApiResponse<WebSpotlightModel>> GetWebSpotlightStatusInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Activates Web Spotlight for the environment.</summary>
    /// <param name="webSpotlightActivateModel">The activation settings.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/web-spotlight")]
    internal Task<IApiResponse<WebSpotlightModel>> ActivateWebSpotlightInternalAsync(
        [Body] WebSpotlightActivateModel webSpotlightActivateModel,
        CancellationToken cancellationToken = default);

    /// <summary>Deactivates Web Spotlight for the environment.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/web-spotlight")]
    internal Task<IApiResponse<WebSpotlightModel>> DeactivateWebSpotlightInternalAsync(
        CancellationToken cancellationToken = default);
}
