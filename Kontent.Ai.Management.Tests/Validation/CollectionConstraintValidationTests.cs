using AwesomeAssertions;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
using Kontent.Ai.Management.Validation;

namespace Kontent.Ai.Management.Tests.Validation;

public class CollectionConstraintValidationTests
{
    [Fact]
    public void NullCollection_NoConstraintFires()
    {
        // [MinElements], [MaxElements], [ExactElements] are all gated on the collection being non-null.
        // A null collection bypasses every count check — required-ness is a separate (publish-time) concern.
        var result = ContentItemValidator.Validate(new Article { Tags = null, Category = null });

        result.ValidationErrors().Should().NotContain(e => e.Path == "tags");
        result.ValidationErrors().Should().NotContain(e => e.Path == "category");
    }

    [Fact]
    public void MinElements_AtLimit_Accepted()
    {
        var result = ContentItemValidator.Validate(new Article { Tags = [ArticleCategory.News] });

        result.ValidationErrors().Should().NotContain(e => e.Path == "tags");
    }

    [Fact]
    public void MinElements_BelowLimit_ProducesError()
    {
        var result = ContentItemValidator.Validate(new Article { Tags = [] });

        var error = result.ValidationErrors().Single(e => e.Path == "tags");
        error.Message.Should().Contain("at least 1").And.Contain("got 0");
    }

    [Fact]
    public void MaxElements_AtLimit_Accepted()
    {
        var tags = Enumerable.Repeat(ArticleCategory.News, 5).ToArray();

        var result = ContentItemValidator.Validate(new Article { Tags = tags });

        result.ValidationErrors().Should().NotContain(e => e.Path == "tags");
    }

    [Fact]
    public void MaxElements_OverLimit_ProducesError()
    {
        var tags = Enumerable.Repeat(ArticleCategory.News, 6).ToArray();

        var result = ContentItemValidator.Validate(new Article { Tags = tags });

        result.ValidationErrors().Single(e => e.Path == "tags")
            .Message.Should().Contain("at most 5").And.Contain("got 6");
    }

    [Fact]
    public void ExactElements_ExactCount_Accepted()
    {
        var result = ContentItemValidator.Validate(new Article { Category = [ArticleCategory.News] });

        result.ValidationErrors().Should().NotContain(e => e.Path == "category");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(5)]
    public void ExactElements_WrongCount_ProducesError(int count)
    {
        var category = Enumerable.Repeat(ArticleCategory.News, count).ToArray();

        var result = ContentItemValidator.Validate(new Article { Category = category });

        result.ValidationErrors().Single(e => e.Path == "category")
            .Message.Should().Contain("exactly 1").And.Contain($"got {count}");
    }
}
