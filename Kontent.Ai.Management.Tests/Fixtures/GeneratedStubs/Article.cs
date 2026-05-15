using System.ComponentModel.DataAnnotations;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;

namespace Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;

// Representative stub of a model-generator-net emitted content-type record. Includes one property per element
// type the validator currently constrains, plus a few un-constrained properties to confirm they're inert.
// Constraint coverage:
//   - [StringLength]        on Title
//   - [RegularExpression]   on Slug
//   - [MinElements]+[MaxElements] on Tags
//   - [ExactElements]       on Category
//   - [AllowedTypes]        on Related
//   - phase-3 no-op attrs   on HeroAssets and Taxonomy (verifying the validator accepts but doesn't enforce)
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
    [MaxAssetSize(10_485_760)]                  // no-op in phase 3
    [AllowedAssetFileTypes(AssetFileType.Image)] // no-op in phase 3
    public IReadOnlyList<AssetReference>? HeroAssets { get; init; }

    [KontentElement("related", "99999999-9999-9999-9999-999999999999")]
    [AllowedTypes("article", "page")]
    public IReadOnlyList<IContentItem>? Related { get; init; }

    [KontentElement("taxonomy", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    [AllowedTaxonomyGroup("categories")]        // no-op in phase 3
    [MinElements(1)]
    public IReadOnlyList<Reference>? Taxonomy { get; init; }
}
