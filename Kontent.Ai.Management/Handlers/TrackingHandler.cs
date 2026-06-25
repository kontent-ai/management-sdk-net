using Kontent.Ai.Management.Extensions;

namespace Kontent.Ai.Management.Handlers;

/// <summary>
/// Adds the SDK and source tracking headers (<c>X-KC-SDKID</c> and optional <c>X-KC-SOURCE</c>) to every outgoing request.
/// </summary>
internal sealed class TrackingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.Headers.AddSdkTrackingHeader();
        request.Headers.AddSourceTrackingHeader();

        return base.SendAsync(request, cancellationToken);
    }
}
