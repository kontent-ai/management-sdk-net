using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<CollectionsModel>> ListCollectionsAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.ListCollectionsInternalAsync(cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<CollectionsModel>> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        return _managementApi.ModifyCollectionInternalAsync(changes, cancellationToken).ToManagementResultAsync();
    }
}
