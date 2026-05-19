using FluentAssertions;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
using Kontent.Ai.Management.Validation;
using Xunit;

namespace Kontent.Ai.Management.Tests.Validation;

public class AllowedTypesValidationTests
{
    [Fact]
    public void NullCollection_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Related = null });

        result.Errors.Should().NotContain(e => e.ElementCodename == "related");
    }

    [Fact]
    public void EmptyCollection_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new Article { Related = [] });

        result.Errors.Should().NotContain(e => e.ElementCodename == "related");
    }

    [Fact]
    public void AllItemsAllowed_NoError()
    {
        var result = ContentItemValidator.Validate(new Article
        {
            Related = [new Article(), new Page(), new Article()],
        });

        result.Errors.Should().NotContain(e => e.ElementCodename == "related");
    }

    [Fact]
    public void DisallowedItem_ProducesError()
    {
        var result = ContentItemValidator.Validate(new Article { Related = [new Banner()] });

        result.Errors.Single(e => e.ElementCodename == "related")
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

        result.Errors.Count(e => e.ElementCodename == "related").Should().Be(2);
    }
}
