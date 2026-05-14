using System.Text.Json;
using FluentAssertions;
using Kontent.Ai.Management.Models.Content;
using Xunit;

namespace Kontent.Ai.Management.Tests.Content;

public class RichTextElementTests
{
    private static readonly Guid SampleComponentId = new("44444444-4444-4444-4444-444444444444");

    [Fact]
    public void RichText_Deserialize_WithoutComponents()
    {
        var json = """{ "value": "<p>hello</p>" }""";

        var element = JsonSerializer.Deserialize<RichTextElement>(json);

        element!.Value.Should().Be("<p>hello</p>");
        element.Components.Should().BeNull();
    }

    [Fact]
    public void RichText_Deserialize_WithComponents()
    {
        var json = $$"""
            {
              "value": "<p>hello</p>",
              "components": [ { "id": "{{SampleComponentId}}" } ]
            }
            """;

        var element = JsonSerializer.Deserialize<RichTextElement>(json);

        element!.Value.Should().Be("<p>hello</p>");
        element.Components.Should().ContainSingle().Which.Id.Should().Be(SampleComponentId);
    }

    [Fact]
    public void RichText_Deserialize_MissingValue_Throws()
    {
        // The `required` keyword on RichTextElement.Value is honored by STJ at deserialize time —
        // a payload that omits "value" is a contract violation, not a partial response.
        var json = """{ "components": [] }""";

        Action act = () => JsonSerializer.Deserialize<RichTextElement>(json);

        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void RichText_RoundTrip_PreservesValue()
    {
        var original = new RichTextElement
        {
            Value = "<p>hello</p>",
            Components = [new Component { Id = SampleComponentId }],
        };

        var json = JsonSerializer.Serialize(original);
        var roundtripped = JsonSerializer.Deserialize<RichTextElement>(json);

        // BeEquivalentTo rather than Be — synthesized record equality compares IReadOnlyList<T> by reference,
        // and the deserialized List<T> is a different instance from the original collection literal.
        roundtripped.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void RichText_RecordEqualityHoldsByValue()
    {
        var a = new RichTextElement { Value = "<p>x</p>" };
        var b = new RichTextElement { Value = "<p>x</p>" };

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Component_Deserialize_Id()
    {
        var json = $$"""{ "id": "{{SampleComponentId}}" }""";

        var component = JsonSerializer.Deserialize<Component>(json);

        component!.Id.Should().Be(SampleComponentId);
    }

    [Fact]
    public void Component_Deserialize_MissingId_Throws()
    {
        var json = "{}";

        Action act = () => JsonSerializer.Deserialize<Component>(json);

        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Component_RoundTrip_PreservesValue()
    {
        var original = new Component { Id = SampleComponentId };

        var json = JsonSerializer.Serialize(original);
        var roundtripped = JsonSerializer.Deserialize<Component>(json);

        roundtripped.Should().Be(original);
    }
}
