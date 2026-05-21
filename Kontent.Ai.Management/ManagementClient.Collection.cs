using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IManagementResult<CollectionsModel>> ListCollectionsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _managementApi.ListCollectionsInternalAsync(cancellationToken);
        return await response.ToManagementResultAsync();
    }

    /// <inheritdoc />
    public async Task<IManagementResult<CollectionsModel>> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        var response = await _managementApi.ModifyCollectionInternalAsync(changes, cancellationToken);
        return await response.ToManagementResultAsync();
    }
}
