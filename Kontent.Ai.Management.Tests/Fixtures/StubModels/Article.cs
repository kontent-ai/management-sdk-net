using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Representative stub of a model-generator-net emitted content-type record, one property per element type.
// Exercised by the envelope-converter and content-type-registry tests.
[KontentType("article", "11111111-1111-1111-1111-111111111111")]
internal sealed record Article : IElementsModel
{
    [KontentElement("title", "22222222-2222-2222-2222-222222222222")]
    public string? Title { get; init; }

    [KontentElement("slug", "33333333-3333-3333-3333-333333333333")]
    public string? Slug { get; init; }

    [KontentElement("body", "44444444-4444-4444-4444-444444444444")]
    public RichTextValue? Body { get; init; }

    [KontentElement("rating", "55555555-5555-5555-5555-555555555555")]
    public decimal? Rating { get; init; }

    [KontentElement("category", "66666666-6666-6666-6666-666666666666")]
    public IEnumerable<ArticleCategory>? Category { get; init; }

    [KontentElement("tags", "77777777-7777-7777-7777-777777777777")]
    public IEnumerable<ArticleCategory>? Tags { get; init; }

    [KontentElement("hero_assets", "88888888-8888-8888-8888-888888888888")]
    public IEnumerable<AssetReference>? HeroAssets { get; init; }

    [KontentElement("related", "99999999-9999-9999-9999-999999999999")]
    public IEnumerable<Reference>? Related { get; init; }

    [KontentElement("taxonomy", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    public IEnumerable<Reference>? Taxonomy { get; init; }
}
