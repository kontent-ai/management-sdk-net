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

        var response = await _managementApi.CloneEnvironmentInternalAsync(cloneEnvironmentModel, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentCloningStateModel>> GetEnvironmentCloningStateAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.GetEnvironmentCloningStateInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> DeleteEnvironmentAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.DeleteEnvironmentInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult> MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(markAsProductionModel);

        var response = await _managementApi.MarkEnvironmentAsProductionInternalAsync(markAsProductionModel, cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<EnvironmentModel>> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyEnvironmentInternalAsync(changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
