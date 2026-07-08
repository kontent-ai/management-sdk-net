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
}
