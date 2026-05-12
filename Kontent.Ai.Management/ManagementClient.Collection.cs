using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<CollectionsModel> ListCollectionsAsync()
        => EnsureSuccess(await _managementApi.ListCollectionsInternalAsync());

    /// <inheritdoc />
    public async Task<CollectionsModel> ModifyCollectionAsync(IEnumerable<CollectionOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);

        return EnsureSuccess(await _managementApi.ModifyCollectionInternalAsync(changes));
    }
}
