using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public Task<IManagementResult<CollectionsModel>> GetCollectionsAsync(CancellationToken cancellationToken = default)
    {
        return _managementApi.GetCollectionsInternalAsync(cancellationToken).ToManagementResultAsync();
    }

    /// <inheritdoc />
    public Task<IManagementResult<CollectionsModel>> ModifyCollectionsAsync(IEnumerable<CollectionOperationBaseModel> changes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(changes);

        return _managementApi.ModifyCollectionsInternalAsync(changes, cancellationToken).ToManagementResultAsync();
    }
}
