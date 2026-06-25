using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using ModelsArticle = MyProject.Models.Article;
using StubsArticle = Kontent.Ai.Management.Tests.Fixtures.StubModels.Article;

namespace Kontent.Ai.Management.Tests.Conversion;

public class ContentTypeRegistryTests
{
    private const string ModelsArticleId = "5568750a-d7fd-51fa-a8bb-a08940ac5395";

    [Fact]
    public void Register_IndexesTypeById()
    {
        var registry = new ContentTypeRegistry();

        registry.Register(typeof(ModelsArticle));

        registry.ResolveById(ModelsArticleId).Should().Be<ModelsArticle>();
    }

    [Fact]
    public void Register_SameTypeTwice_IsIdempotent()
    {
        var registry = new ContentTypeRegistry();

        registry.Register(typeof(ModelsArticle));
        registry.Register(typeof(ModelsArticle));

        registry.ResolveById(ModelsArticleId).Should().Be<ModelsArticle>();
    }

    [Fact]
    public void Register_CollidingCodename_Throws()
    {
        // Two types share `[KontentType("article")]` — registering the second should refuse and surface both names.
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(ModelsArticle));

        var act = () => registry.Register(typeof(StubsArticle));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*article*");
    }

    [Fact]
    public void EnsureRegistered_SameTypeTwice_IsIdempotent()
    {
        var registry = new ContentTypeRegistry();

        registry.EnsureRegistered(typeof(ModelsArticle));
        registry.EnsureRegistered(typeof(ModelsArticle));

        registry.ResolveById(ModelsArticleId).Should().Be<ModelsArticle>();
    }

    [Fact]
    public void EnsureRegistered_CollidingCodename_Throws()
    {
        // Same codename, different type — the read-path entry must stay collision-strict, not silently keep the first mapping.
        var registry = new ContentTypeRegistry();
        registry.EnsureRegistered(typeof(ModelsArticle));

        var act = () => registry.EnsureRegistered(typeof(StubsArticle));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*article*");
    }

    [Fact]
    public void EnsureRegistered_NonContentType_IsNoOp()
    {
        // Unlike Register, the read-path entry tolerates a root that isn't a content-type record.
        var registry = new ContentTypeRegistry();

        var act = () => registry.EnsureRegistered(typeof(string));

        act.Should().NotThrow();
        registry.ResolveById(ModelsArticleId).Should().BeNull();
    }

    [Fact]
    public void ResolveById_UnknownId_ReturnsNull()
    {
        var registry = new ContentTypeRegistry();

        registry.ResolveById("00000000-0000-0000-0000-000000000000").Should().BeNull();
    }

    [Fact]
    public void Register_NonContentTypeRecord_Throws()
    {
        var registry = new ContentTypeRegistry();

        var act = () => registry.Register(typeof(string));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Scan_OnCollidingAssembly_ThrowsLoudly()
    {
        // This test assembly co-locates two content-model sets, StubModels and MyProject.Models, that share codenames.
        // Scan must surface that collision instead of silently keeping whichever type it saw first. Real projects use
        // unique codenames and never hit this, so the test exists to pin the contract.
        var registry = new ContentTypeRegistry();

        var act = () => registry.Scan(typeof(ModelsArticle).Assembly);

        act.Should().Throw<InvalidOperationException>();
    }
}
