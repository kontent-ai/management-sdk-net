using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using MyProject.Models;
using Xunit;

namespace Kontent.Ai.Management.Tests.Conversion;

public class EnvelopeConverter_ReadTests
{
    private static readonly ContentItemEnvelopeConverter Converter = new();

    [Fact]
    public void EmptyArray_ReturnsEmptyRecord()
    {
        var article = Converter.ReadEnvelopes<Article>("[]");

        article.Title.Should().BeNull();
        article.Abstract.Should().BeNull();
    }

    [Fact]
    public void UnknownElement_IsSkipped()
    {
        // Forward-compat: a future MAPI response containing an element our record doesn't know about must not throw.
        var json = """
            [
              { "element": { "id": "ae9be828-90fe-5fb4-ae3a-8dfe047e2567" }, "value": "Hello" },
              { "element": { "id": "unknown-id-not-in-record" }, "value": "ignored" }
            ]
            """;

        var article = Converter.ReadEnvelopes<Article>(json);

        article.Title.Should().Be("Hello");
    }

    [Fact]
    public void Text_RoundTrips()
    {
        var json = """[{"element":{"id":"ae9be828-90fe-5fb4-ae3a-8dfe047e2567"},"value":"Hello"}]""";

        Converter.ReadEnvelopes<Article>(json).Title.Should().Be("Hello");
    }

    [Fact]
    public void Number_RoundTrips()
    {
        var json = """[{"element":{"id":"e80d04c6-7f54-5b7a-b0ec-6c511138ed06"},"value":42.5}]""";

        Converter.ReadEnvelopes<Product>(json).Price.Should().Be(42.5m);
    }

    [Fact]
    public void DateTime_RoundTrips()
    {
        var json = """[{"element":{"id":"dba8efd9-052c-557a-9dfd-3f43f966eab8"},"value":"2024-06-01T12:00:00+00:00"}]""";

        Converter.ReadEnvelopes<Article>(json).PublishingDate
            .Should().Be(new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void MultipleChoice_ResolvesByCodenameAndId()
    {
        var json = """
            [{
              "element": { "id": "44dd9032-c950-53b8-91bd-c6c586233311" },
              "value": [
                { "codename": "warning" },
                { "id": "182912d7-d863-5d4a-8a5a-758c4acb2cb7" }
              ]
            }]
            """;

        var callout = Converter.ReadEnvelopes<Callout>(json);

        callout.Type.Should().Equal(CalloutType.Warning, CalloutType.Info);
    }

    [Fact]
    public void MultipleChoice_UnknownOption_IsSkipped()
    {
        var json = """
            [{
              "element": { "id": "44dd9032-c950-53b8-91bd-c6c586233311" },
              "value": [
                { "codename": "warning" },
                { "codename": "unknown-option" }
              ]
            }]
            """;

        var callout = Converter.ReadEnvelopes<Callout>(json);

        callout.Type.Should().Equal(CalloutType.Warning);
    }

    [Fact]
    public void Asset_RoundTrips()
    {
        var id = Guid.NewGuid();
        var json = $$"""
            [{
              "element": { "id": "ed868ae7-0b90-5752-a53b-aa4cfa2ca56e" },
              "value": [ { "id": "{{id}}" } ]
            }]
            """;

        var article = Converter.ReadEnvelopes<Article>(json);

        article.HeroImage.Should().ContainSingle().Which.Id.Should().Be(id);
    }

    [Fact]
    public void Reference_RoundTrips()
    {
        var id = Guid.NewGuid();
        var json = $$"""
            [{
              "element": { "id": "c505fded-9c1f-5583-8cf2-dfa4763d2f2a" },
              "value": [ { "id": "{{id}}" } ]
            }]
            """;

        var article = Converter.ReadEnvelopes<Article>(json);

        article.Type.Should().ContainSingle().Which.Id.Should().Be(id);
    }

    [Fact]
    public void RichText_WithoutComponents_RoundTrips()
    {
        var json = """
            [{
              "element": { "id": "4341b423-f053-523f-bd0f-b63b6c786532" },
              "value": "<p>hello</p>"
            }]
            """;

        var person = Converter.ReadEnvelopes<Person>(json);

        person.Bio!.Value.Should().Be("<p>hello</p>");
        person.Bio.Components.Should().BeNull();
    }

    [Fact]
    public void Read_MatchesByCodenameWhenIdAbsent()
    {
        // The wire format on writes is codename-keyed; the converter accepts that shape on reads too.
        var json = """[{"element":{"codename":"title"},"value":"FromCodename"}]""";

        Converter.ReadEnvelopes<Article>(json).Title.Should().Be("FromCodename");
    }
}
