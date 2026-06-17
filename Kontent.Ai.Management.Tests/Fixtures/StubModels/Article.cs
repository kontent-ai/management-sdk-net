using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;
using System.ComponentModel.DataAnnotations;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Representative stub of a model-generator-net emitted content-type record. Includes one property per element
// type the validator currently constrains, plus a few un-constrained properties to confirm they're inert.
// Constraint coverage:
//   - [StringLength]        on Title
//   - [RegularExpression]   on Slug
//   - [MinElements]+[MaxElements] on Tags
//   - [ExactElements]       on Category
//   - no-op attrs           on HeroAssets, Taxonomy and Related (validator accepts but doesn't enforce —
//                           linked-item refs carry no type, so [AllowedTypes] there is server-enforced)
[KontentType("article", "11111111-1111-1111-1111-111111111111")]
internal sealed record Article : IContentItem
{
    [KontentElement("title", "22222222-2222-2222-2222-222222222222")]
    [StringLength(50)]
    public string? Title { get; init; }

    [KontentElement("slug", "33333333-3333-3333-3333-333333333333")]
    [RegularExpression("^[a-z0-9-]+$")]
    public string? Slug { get; init; }

    [KontentElement("body", "44444444-4444-4444-4444-444444444444")]
    public RichTextElement? Body { get; init; }

    [KontentElement("rating", "55555555-5555-5555-5555-555555555555")]
    public decimal? Rating { get; init; }

    [KontentElement("category", "66666666-6666-6666-6666-666666666666")]
    [ExactElements(1)]
    public IReadOnlyList<ArticleCategory>? Category { get; init; }

    [KontentElement("tags", "77777777-7777-7777-7777-777777777777")]
    [MinElements(1)]
    [MaxElements(5)]
    public IReadOnlyList<ArticleCategory>? Tags { get; init; }

    [KontentElement("hero_assets", "88888888-8888-8888-8888-888888888888")]
    [MaxElements(3)]
    [MaxAssetSize(10_485_760)]                  // not enforced by the validator
    [AllowedAssetFileTypes(AssetFileType.Image)] // not enforced by the validator
    public IReadOnlyList<AssetReference>? HeroAssets { get; init; }

    [KontentElement("related", "99999999-9999-9999-9999-999999999999")]
    [AllowedTypes("article", "page")]           // not enforced by the validator — linked items are bare references
    public IReadOnlyList<Reference>? Related { get; init; }

    [KontentElement("taxonomy", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    [AllowedTaxonomyGroup("categories")]        // not enforced by the validator
    [MinElements(1)]
    public IReadOnlyList<Reference>? Taxonomy { get; init; }
}
