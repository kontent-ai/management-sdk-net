using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentClonedModel>> CloneEnvironmentAsync(EnvironmentCloneModel cloneEnvironmentModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(cloneEnvironmentModel);

        var response = await _managementApi.CloneEnvironmentInternalAsync(cloneEnvironmentModel, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentCloningStateModel>> GetEnvironmentCloningStateAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetEnvironmentCloningStateInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteEnvironmentAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.DeleteEnvironmentInternalAsync(cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult> MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(markAsProductionModel);

        var response = await _managementApi.MarkEnvironmentAsProductionInternalAsync(markAsProductionModel, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentModel>> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyEnvironmentInternalAsync(changes, cancellationToken).ConfigureAwait(false);
        return await response.ToManagementResultAsync().ConfigureAwait(false);
    }
}
