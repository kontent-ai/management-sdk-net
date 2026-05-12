using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<SpaceModel> CreateSpaceAsync(SpaceCreateModel space)
    {
        ArgumentNullException.ThrowIfNull(space);

        return EnsureSuccess(await _managementApi.CreateSpaceInternalAsync(space));
    }

    /// <inheritdoc />
    public async Task<SpaceModel> GetSpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetSpaceInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SpaceModel>> ListSpacesAsync()
        => EnsureSuccess(await _managementApi.ListSpacesInternalAsync());

    /// <inheritdoc />
    public async Task<SpaceModel> ModifySpaceAsync(Reference identifier, IEnumerable<SpaceOperationReplaceModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);
        ArgumentNullException.ThrowIfNull(changes);

        return EnsureSuccess(await _managementApi.ModifySpaceInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename), changes));
    }

    /// <inheritdoc />
    public async Task DeleteSpaceAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteSpaceInternalAsync(identifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)));
    }
}
