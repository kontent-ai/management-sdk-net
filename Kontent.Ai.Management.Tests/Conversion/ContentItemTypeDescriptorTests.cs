using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using StubsArticle = Kontent.Ai.Management.Tests.Fixtures.StubModels.Article;

namespace Kontent.Ai.Management.Tests.Conversion;

public class ContentItemTypeDescriptorTests
{
    // Ids are GUID strings and the wire may carry them in any casing; a hand-written or tool-emitted model with an
    // uppercase [KontentElement] id must still bind — a case-sensitive map would silently skip the element on reads.
    [Fact]
    public void ByElementId_MatchesCaseInsensitively()
    {
        var descriptor = ContentItemTypeDescriptor.For(typeof(StubsArticle));

        descriptor.ByElementId.Should().ContainKey("22222222-2222-2222-2222-222222222222");
        descriptor.ByElementId.Should().ContainKey("22222222-2222-2222-2222-222222222222".ToUpperInvariant());
    }

    [Fact]
    public void ByElementCodename_StaysCaseSensitive()
    {
        var descriptor = ContentItemTypeDescriptor.For(typeof(StubsArticle));

        descriptor.ByElementCodename.Should().ContainKey("title");
        descriptor.ByElementCodename.Should().NotContainKey("TITLE");
    }
}
