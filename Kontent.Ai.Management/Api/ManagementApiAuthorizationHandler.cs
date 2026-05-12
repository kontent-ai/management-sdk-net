using System.Net.Http.Headers;
using Kontent.Ai.Management.Modules.Extensions;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Attaches the Management API bearer token and the SDK / source tracking headers to every outgoing request.
/// Mirrors the headers the legacy <c>MessageCreator</c> adds.
/// </summary>
internal sealed class ManagementApiAuthorizationHandler(string apiKey) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Headers.AddSdkTrackingHeader();
        request.Headers.AddSourceTrackingHeader();

        return base.SendAsync(request, cancellationToken);
    }
}
