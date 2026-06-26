using RichardSzalay.MockHttp;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>Holds the raw request body captured by <see cref="MockHttpExtensions.CaptureBody"/>.</summary>
internal sealed class CapturedBody
{
    public string? Value { get; set; }
}

internal static class MockHttpExtensions
{
    /// <summary>
    /// Captures the request body into <paramref name="body"/> as the expectation is matched, so a test can assert
    /// what the SDK actually sent. Replaces the hand-rolled sync-over-async <c>.With(...)</c> capture block.
    /// </summary>
    public static MockedRequest CaptureBody(this MockedRequest request, out CapturedBody body)
    {
        var captured = new CapturedBody();
        body = captured;
        return request.With(r =>
        {
            captured.Value = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
            return true;
        });
    }
}
