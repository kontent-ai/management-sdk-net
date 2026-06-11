using AwesomeAssertions;
using Kontent.Ai.Management.Tests.Fixtures.StubModels;
using Kontent.Ai.Management.Validation;

namespace Kontent.Ai.Management.Tests.Validation;

public class AllowedTypesValidationTests
{
    [Fact]
    public void NullCollection_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Related = null });

        result.ValidationErrors().Should().NotContain(e => e.Path == "related");
    }

    [Fact]
    public void EmptyCollection_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Related = [] });

        result.ValidationErrors().Should().NotContain(e => e.Path == "related");
    }

    [Fact]
    public void AllItemsAllowed_NoError()
    {
        var result = ContentItemValidator.Validate(new Article
        {
            Related = [new Article(), new Page(), new Article()],
        });

        result.ValidationErrors().Should().NotContain(e => e.Path == "related");
    }

    [Fact]
    public void DisallowedItem_ProducesError()
    {
        var result = ContentItemValidator.Validate(new Article { Related = [new Banner()] });

        result.ValidationErrors().Single(e => e.Path == "related")
            .Message.Should().Contain("'banner'").And.Contain("article").And.Contain("page");
    }

    [Fact]
    public void MultipleDisallowedItems_ProduceMultipleErrors()
    {
        // The validator emits one error per disallowed entry; consumers can dedupe in their UI if needed.
        var result = ContentItemValidator.Validate(new Article
        {
            Related = [new Banner(), new Article(), new Banner()],
        });

        result.ValidationErrors().Count(e => e.Path == "related").Should().Be(2);
    }
}
