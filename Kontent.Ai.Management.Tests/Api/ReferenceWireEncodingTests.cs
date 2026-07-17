using AwesomeAssertions;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Net;

namespace Kontent.Ai.Management.Tests.Api;

// Regression guard for the codename/external-id double-encoding bug. ToUrlSegment leaves the value raw and Refit's
// {**} catch-all encodes it exactly once, so reserved characters must reach the wire encoded a single time. The
// unit tests of ToUrlSegment can't catch this — only the composed request URI reveals the double encode.
public class ReferenceWireEncodingTests
{
    [Fact]
    public async Task GetAsset_ByExternalIdWithSpace_EncodesExactlyOnceOnTheWire()
    {
        var (client, mock) = MockClientFactory.Create();
        string? wire = null;
        mock.When("*").With(r => { wire = r.RequestUri!.AbsoluteUri; return true; })
            .Respond(HttpStatusCode.InternalServerError, "application/json", "{}");

        await client.GetAssetAsync(Reference.ByExternalId("with space"));

        wire.Should().EndWith("/assets/external-id/with%20space");
        wire.Should().NotContain("%2520", "the segment must be encoded once, not double-encoded");
    }

    [Fact]
    public async Task GetAsset_ByExternalIdWithAtSign_EncodesExactlyOnceOnTheWire()
    {
        var (client, mock) = MockClientFactory.Create();
        string? wire = null;
        mock.When("*").With(r => { wire = r.RequestUri!.AbsoluteUri; return true; })
            .Respond(HttpStatusCode.InternalServerError, "application/json", "{}");

        await client.GetAssetAsync(Reference.ByExternalId("user@kontent.ai"));

        wire.Should().EndWith("/assets/external-id/user%40kontent.ai");
    }

    // The traversal guard (EnsureSingleSegment) rejects literal dot-segments but deliberately lets '%' through — its
    // safety against *percent-encoded* dot-segments ("%2e%2e") rests entirely on the catch-all encoding '%' itself,
    // so the payload reaches the wire inert as "%252e%252e". This test pins that Refit behavior: if an upgrade ever
    // stops encoding '%', the guard is bypassable and this fails.
    [Fact]
    public async Task GetAsset_ByExternalIdWithEncodedDotSegments_ReachesWireDoubleEncoded()
    {
        var (client, mock) = MockClientFactory.Create();
        string? wire = null;
        mock.When("*").With(r => { wire = r.RequestUri!.AbsoluteUri; return true; })
            .Respond(HttpStatusCode.InternalServerError, "application/json", "{}");

        await client.GetAssetAsync(Reference.ByExternalId("%2e%2e%2fwebhooks-vnext"));

        wire.Should().EndWith("/assets/external-id/%252e%252e%252fwebhooks-vnext");
    }
}
