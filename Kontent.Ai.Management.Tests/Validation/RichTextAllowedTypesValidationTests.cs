using AwesomeAssertions;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Models.Content;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
using Kontent.Ai.Management.Validation;
using Xunit;

namespace Kontent.Ai.Management.Tests.Validation;

public class RichTextAllowedTypesValidationTests
{
    private static Component Embed(IContentItem content) =>
        new() { Id = Guid.NewGuid(), Content = content };

    [Fact]
    public void NullRichText_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new RichTextHost { Body = null });

        result.ValidationErrors().Should().NotContain(e => e.Path == "body");
    }

    [Fact]
    public void RichTextWithoutComponents_IsAccepted()
    {
        var result = ContentItemValidator.Validate(new RichTextHost
        {
            Body = new RichTextElement { Value = "<p>plain</p>" },
        });

        result.ValidationErrors().Should().NotContain(e => e.Path == "body");
    }

    [Fact]
    public void AllowedComponentTypes_NoError()
    {
        var result = ContentItemValidator.Validate(new RichTextHost
        {
            Body = new RichTextElement
            {
                Value = "<p>x</p>",
                Components = [Embed(new Page()), Embed(new Article())],
            },
        });

        result.ValidationErrors().Should().NotContain(e => e.Path == "body");
    }

    [Fact]
    public void DisallowedComponentType_ProducesError()
    {
        var result = ContentItemValidator.Validate(new RichTextHost
        {
            Body = new RichTextElement
            {
                Value = "<p>x</p>",
                Components = [Embed(new Banner())],
            },
        });

        result.ValidationErrors().Single(e => e.Path == "body")
            .Message.Should().Contain("'banner'").And.Contain("page").And.Contain("article");
    }

    [Fact]
    public void MultipleDisallowedComponents_ProduceMultipleErrors()
    {
        // One error per disallowed component, symmetric with the linked-items [AllowedTypes] rule.
        var result = ContentItemValidator.Validate(new RichTextHost
        {
            Body = new RichTextElement
            {
                Value = "<p>x</p>",
                Components = [Embed(new Banner()), Embed(new Page()), Embed(new Banner())],
            },
        });

        result.ValidationErrors().Count(e => e.Path == "body").Should().Be(2);
    }
}
