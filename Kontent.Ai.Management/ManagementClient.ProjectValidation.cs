using Kontent.Ai.Management.Models.EnvironmentReport;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<EnvironmentReportModel> ValidateEnvironmentAsync()
    {
        var endpointUrl = _urlBuilder.BuildValidationUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<EnvironmentReportModel>(endpointUrl, HttpMethod.Post);
    }

    /// <inheritdoc />
    public async Task<AsyncValidationTaskModel> InitiateEnvironmentAsyncValidationTaskAsync()
    {
        var endpointUrl = _urlBuilder.BuildAsyncValidationUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<AsyncValidationTaskModel>(endpointUrl, HttpMethod.Post);
    }

    /// <inheritdoc />
    public async Task<AsyncValidationTaskModel> GetAsyncValidationTaskAsync(Guid taskId)
    {
        var endpointUrl = _urlBuilder.BuildAsyncValidationTaskUrl(taskId);
        return await _actionInvoker.InvokeReadOnlyMethodAsync<AsyncValidationTaskModel>(endpointUrl, HttpMethod.Get);
    }

    /// <inheritdoc />
    public async Task<IListingResponseModel<AsyncValidationTaskIssueModel>> ListAsyncValidationTaskIssuesAsync(Guid taskId)
    {
        var endpointUrl = _urlBuilder.BuildAsyncValidationTaskIssuesUrl(taskId);

        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<AsyncValidationTaskIssuesResponseServerModel>(endpointUrl, HttpMethod.Get);

        return new ListingResponseModel<AsyncValidationTaskIssueModel>(
                GetNextListingPageAsync<AsyncValidationTaskIssuesResponseServerModel, AsyncValidationTaskIssueModel>,
                response.Pagination?.Token,
                endpointUrl,
                response.Issues);
    }
}
