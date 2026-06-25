using AwesomeAssertions;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Conversion;
using ModelsArticle = MyProject.Models.Article;
using StubsArticle = Kontent.Ai.Management.Tests.Fixtures.StubModels.Article;

namespace Kontent.Ai.Management.Tests.Conversion;

public class ContentTypeRegistryTests
{
    private const string ModelsArticleId = "5568750a-d7fd-51fa-a8bb-a08940ac5395";
    private const string StubsArticleId = "11111111-1111-1111-1111-111111111111";

    // A second content-type record deliberately claiming ModelsArticle's id, to exercise id-collision detection.
    [KontentType("clashing", ModelsArticleId)]
    private sealed record IdClashingArticle : IElementsModel;

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
    public void Register_SharedCodenameDistinctIds_ResolvesEachById()
    {
        // Two types share `[KontentType("article")]` but carry distinct ids — resolution is by id, so both coexist.
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(ModelsArticle));
        registry.Register(typeof(StubsArticle));

        registry.ResolveById(ModelsArticleId).Should().Be<ModelsArticle>();
        registry.ResolveById(StubsArticleId).Should().Be<StubsArticle>();
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
    public void EnsureRegistered_CollidingId_Throws()
    {
        // Two types claiming one id cannot both be resolved — the read-path entry stays strict on id, not codename.
        var registry = new ContentTypeRegistry();
        registry.EnsureRegistered(typeof(ModelsArticle));

        var act = () => registry.EnsureRegistered(typeof(IdClashingArticle));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"*{ModelsArticleId}*");
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
    public void Scan_OnIdCollidingAssembly_ThrowsLoudly()
    {
        // Shared codenames across co-located model sets are fine — resolution is by id. An id claimed by two types is
        // not: this assembly's IdClashingArticle deliberately reuses ModelsArticle's id, and Scan must surface that.
        var registry = new ContentTypeRegistry();

        var act = () => registry.Scan(typeof(ModelsArticle).Assembly);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"*{ModelsArticleId}*");
    }
}
