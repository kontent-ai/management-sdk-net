using System.ComponentModel.DataAnnotations;
using System.Net;
using FluentAssertions;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
using Kontent.Ai.Management.Validation;
using Xunit;

namespace Kontent.Ai.Management.Tests.Validation;

public class ContentItemValidatorTests
{
    [Fact]
    public void NullItem_Throws()
    {
        Action act = () => _ = ContentItemValidator.Validate<Article>(null!);

        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("item");
    }

    [Fact]
    public void EmptyItem_IsAccepted()
    {
        // No properties set — every check guards on non-null, so everything passes.
        // (Required-ness is a publish-time concern enforced by the API, not by this validator.)
        var result = ContentItemValidator.Validate(new Article());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
        result.StatusCode.Should().BeNull();
    }

    [Fact]
    public void FullyValidItem_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article
        {
            Title = "Hello",
            Slug = "hello-world",
            Tags = [ArticleCategory.News, ArticleCategory.Release],
            Category = [ArticleCategory.News],
            Related = [new Article(), new Page()],
            HeroAssets = [new AssetReference { Codename = "logo" }],
            Taxonomy = [Reference.ByCodename("news")],
        });

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void MultipleViolations_AreAllCollected()
    {
        var result = ContentItemValidator.Validate(new Article
        {
            Title = new string('x', 51),       // exceeds [StringLength(50)]
            Slug = "BAD slug",                  // fails [RegularExpression]
            Tags = [],                          // below [MinElements(1)]
            Category = [ArticleCategory.News, ArticleCategory.Release], // not [ExactElements(1)]
            Related = [new Banner()],           // not in [AllowedTypes]
        });

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.StatusCode.Should().BeNull();

        var codenames = result.Errors.Select(e => e.ElementCodename).ToList();
        codenames.Should().Contain("title");
        codenames.Should().Contain("slug");
        codenames.Should().Contain("tags");
        codenames.Should().Contain("category");
        codenames.Should().Contain("related");
    }

    [Fact]
    public void NoOpAttributes_DoNotFireInPhase3()
    {
        // [MaxAssetSize], [AllowedAssetFileTypes], [AllowedTaxonomyGroup] are recorded on the property but
        // not enforced — the data needed to check them isn't present in an in-memory record. The validator
        // accepts plausibly-out-of-bounds values for these attributes without complaint.
        var result = ContentItemValidator.Validate(new Article
        {
            // A "huge" hero asset reference — phase 3 has no asset metadata, so [MaxAssetSize] can't fire.
            HeroAssets = [new AssetReference { Codename = "very-large-asset" }],
            // A taxonomy term that may or may not belong to the [AllowedTaxonomyGroup] — can't be checked locally.
            Taxonomy = [Reference.ByCodename("unknown-term")],
        });

        result.Errors.Should().NotContain(e => e.ElementCodename == "hero_assets");
        result.Errors.Should().NotContain(e => e.ElementCodename == "taxonomy");
    }

    [Fact]
    public void Value_OnSuccess_IsSameReference()
    {
        var item = new Article { Title = "Hello" };

        var result = ContentItemValidator.Validate(item);

        result.Value.Should().BeSameAs(item);
    }

    [Fact]
    public void StatusCode_IsAlwaysNull_FromValidator()
    {
        // The validator never makes an HTTP call, so StatusCode is null on both success and failure paths.
        // (Phase 4's strongly-typed client methods will populate it when the failure originates from an API response.)
        var success = ContentItemValidator.Validate(new Article());
        var failure = ContentItemValidator.Validate(new Article { Title = new string('x', 100) });

        success.StatusCode.Should().BeNull();
        failure.StatusCode.Should().BeNull();
        // Sanity: HttpStatusCode is reachable so this test's intent is clearly about its absence, not a typo.
        ((HttpStatusCode?)null).Should().Be(failure.StatusCode);
    }

    [Fact]
    public void RuleCache_IsReusableAcrossCalls()
    {
        // Two validations of the same type should yield consistent results — the cached rule list is stateless
        // and re-runs on each call. Regression guard against a future "rules accidentally mutated by validation" bug.
        var first = ContentItemValidator.Validate(new Article { Title = new string('x', 51) });
        var second = ContentItemValidator.Validate(new Article { Title = new string('y', 51) });

        first.IsSuccess.Should().BeFalse();
        second.IsSuccess.Should().BeFalse();
        first.Errors.Count.Should().Be(second.Errors.Count);
    }

    [KontentType("unannotated_property_owner")]
    private sealed record UnannotatedPropertyOwner : IContentItem
    {
        // No [KontentElement] — must be ignored even with constraint attrs.
        [StringLength(1)]
        public string? IgnoreMe { get; init; }
    }

    [Fact]
    public void PropertiesWithoutKontentElement_AreIgnored()
    {
        // Constraint attrs on a property without [KontentElement] are inert — without a codename, the validator
        // can't produce a meaningful error and silently skips the property.
        var result = ContentItemValidator.Validate(
            new UnannotatedPropertyOwner { IgnoreMe = "way too long" });

        result.IsSuccess.Should().BeTrue();
    }
}
