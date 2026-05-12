using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Patch;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<IListingResponseModel<ContentTypeModel>> ListContentTypesAsync()
    {
        var response = EnsureSuccess(await _managementApi.ListContentTypesInternalAsync());

        return new ListingResponseModel<ContentTypeModel>(
            (continuationToken, _) => GetNextContentTypesPageAsync(continuationToken),
            response.Pagination?.Token,
            url: string.Empty,
            response.Types);
    }

    private async Task<IListingResponse<ContentTypeModel>> GetNextContentTypesPageAsync(string continuationToken)
        => EnsureSuccess(await _managementApi.ListContentTypesInternalAsync(continuationToken));

    /// <inheritdoc />
    public async Task<ContentTypeModel> GetContentTypeAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return EnsureSuccess(await _managementApi.GetContentTypeInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<ContentTypeModel> CreateContentTypeAsync(ContentTypeCreateModel contentType)
    {
        ArgumentNullException.ThrowIfNull(contentType);

        return EnsureSuccess(await _managementApi.CreateContentTypeInternalAsync(contentType));
    }

    /// <inheritdoc />
    public async Task DeleteContentTypeAsync(Reference identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        EnsureSuccess(await _managementApi.DeleteContentTypeInternalAsync(identifier.ToUrlSegment()));
    }

    /// <inheritdoc />
    public async Task<ContentTypeModel> ModifyContentTypeAsync(Reference identifier, IEnumerable<ContentTypeOperationBaseModel> changes)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (changes == null || !changes.Any())
        {
            throw new ArgumentException("Please provide at least one operation.", nameof(changes));
        }

        return EnsureSuccess(await _managementApi.ModifyContentTypeInternalAsync(identifier.ToUrlSegment(), changes));
    }
}
