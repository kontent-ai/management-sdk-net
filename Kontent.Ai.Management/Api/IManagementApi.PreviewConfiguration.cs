using Kontent.Ai.Management.Models.PreviewConfiguration;

namespace Kontent.Ai.Management.Api;

/// <inheritdoc cref="IManagementApi"/>
internal partial interface IManagementApi
{
    /// <summary>Gets the environment's preview configuration.</summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Get("/preview-configuration")]
    internal Task<IApiResponse<PreviewConfigurationModel>> GetPreviewConfigurationInternalAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Replaces the environment's preview configuration.</summary>
    /// <param name="previewConfiguration">The preview configuration to set.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [Put("/preview-configuration")]
    internal Task<IApiResponse<PreviewConfigurationModel>> ModifyPreviewConfigurationInternalAsync(
        [Body] PreviewConfigurationModel previewConfiguration,
        CancellationToken cancellationToken = default);
}
