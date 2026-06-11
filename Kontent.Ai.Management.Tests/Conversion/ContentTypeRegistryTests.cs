using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using ModelsArticle = MyProject.Models.Article;
using StubsArticle = Kontent.Ai.Management.Tests.Fixtures.StubModels.Article;

namespace Kontent.Ai.Management.Tests.Conversion;

public class ContentTypeRegistryTests
{
    [Fact]
    public void Register_IndexesTypeByCodename()
    {
        var registry = new ContentTypeRegistry();

        registry.Register(typeof(ModelsArticle));

        registry.Resolve("article").Should().Be<ModelsArticle>();
    }

    [Fact]
    public void Register_SameTypeTwice_IsIdempotent()
    {
        var registry = new ContentTypeRegistry();

        registry.Register(typeof(ModelsArticle));
        registry.Register(typeof(ModelsArticle));

        registry.Resolve("article").Should().Be<ModelsArticle>();
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
    public void Resolve_UnknownCodename_ReturnsNull()
    {
        var registry = new ContentTypeRegistry();

        registry.Resolve("does_not_exist").Should().BeNull();
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
        // The current test assembly co-locates two content-model sets (StubModels + MyProject.Models) with overlapping codenames;
        // Scan must surface the collision rather than silently last-write-wins. Production assemblies have
        // unique codenames and won't trip this — the test pins the contract.
        var registry = new ContentTypeRegistry();

        var act = () => registry.Scan(typeof(ModelsArticle).Assembly);

        act.Should().Throw<InvalidOperationException>();
    }
}
