using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<EnvironmentClonedModel> CloneEnvironmentAsync(EnvironmentCloneModel cloneEnvironmentModel)
    {
        ArgumentNullException.ThrowIfNull(cloneEnvironmentModel);

        return EnsureSuccess(await _managementApi.CloneEnvironmentInternalAsync(cloneEnvironmentModel));
    }

    /// <inheritdoc />
    public async Task<EnvironmentCloningStateModel> GetEnvironmentCloningStateAsync()
        => EnsureSuccess(await _managementApi.GetEnvironmentCloningStateInternalAsync());

    /// <inheritdoc />
    public async Task DeleteEnvironmentAsync()
        => EnsureSuccess(await _managementApi.DeleteEnvironmentInternalAsync());

    /// <inheritdoc />
    public async Task MarkEnvironmentAsProductionAsync(MarkAsProductionModel markAsProductionModel)
    {
        ArgumentNullException.ThrowIfNull(markAsProductionModel);

        EnsureSuccess(await _managementApi.MarkEnvironmentAsProductionInternalAsync(markAsProductionModel));
    }

    /// <inheritdoc />
    public async Task<EnvironmentModel> ModifyEnvironmentAsync(IEnumerable<EnvironmentOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);

        return EnsureSuccess(await _managementApi.ModifyEnvironmentInternalAsync(changes));
    }
}
