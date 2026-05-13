namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Innermost handler for a Refit-backed <see cref="Kontent.Ai.Management.Api.IManagementApi"/> in tests: replays the
/// configured responses in order (falling back to an empty <c>200 OK</c>) and optionally records the outgoing request.
/// </summary>
/// <remarks>
/// The configured <paramref name="responses"/> are treated as templates — a fresh <see cref="HttpResponseMessage"/> is
/// returned per call so the test can make more calls than templates without hitting a disposed/consumed response.
/// </remarks>
internal sealed class RefitMockHandler(
    IReadOnlyList<HttpResponseMessage> responses,
    Action<HttpRequestMessage, string?>? onRequest = null) : HttpMessageHandler
{
    private int _index;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (onRequest is not null)
        {
            var payload = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            onRequest(request, payload);
        }

        // A real HttpClientHandler always sets RequestMessage; Refit's error-path handling NREs without it.
        if (responses.Count == 0)
        {
            return new HttpResponseMessage { RequestMessage = request };
        }

        var template = responses[Math.Min(_index++, responses.Count - 1)];
        var clone = new HttpResponseMessage(template.StatusCode) { RequestMessage = request };
        if (template.Content is not null)
        {
            clone.Content = new StringContent(await template.Content.ReadAsStringAsync(cancellationToken));
        }

        return clone;
    }
}
