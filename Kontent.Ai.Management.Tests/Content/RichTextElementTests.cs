using System.Text.Json;
using FluentAssertions;
using Kontent.Ai.Management.Models.Content;
using Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;
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
        // Round-trip without components — direct STJ doesn't round-trip Component.Content (it's [JsonIgnore]
        // by design; the phase-4 envelope converter owns the components-with-content story).
        var original = new RichTextElement { Value = "<p>hello</p>" };

        var json = JsonSerializer.Serialize(original);
        var roundtripped = JsonSerializer.Deserialize<RichTextElement>(json);

        roundtripped.Should().Be(original);
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
    public void Component_CarriesIdAndContent()
    {
        var embedded = new Page();
        var component = new Component { Id = SampleComponentId, Content = embedded };

        component.Id.Should().Be(SampleComponentId);
        component.Content.Should().BeSameAs(embedded);
    }

    [Fact]
    public void Component_SerializesIdOnly_ContentIsJsonIgnored()
    {
        // [JsonIgnore] on Content is deliberate: IContentItem is polymorphic and the wire envelope shape
        // (id + type + elements) is the envelope converter's responsibility, not direct STJ's.
        var component = new Component { Id = SampleComponentId, Content = new Page() };

        var json = JsonSerializer.Serialize(component);

        json.Should().Contain("\"id\":");
        json.Should().NotContain("content");
    }
}
