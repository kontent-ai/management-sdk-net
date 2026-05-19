using System.Text.Json;
using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Models.Content;
using MyProject.Models;
using Xunit;

namespace Kontent.Ai.Management.Tests.Conversion;

public class EnvelopeConverter_WriteTests
{
    private static readonly ContentItemEnvelopeConverter Converter = new();

    [Fact]
    public void EmptyRecord_WritesEmptyArray()
    {
        var json = Converter.WriteEnvelopes(new Article());

        json.Should().Be("[]");
    }

    [Fact]
    public void NullProperties_AreSkipped()
    {
        var article = new Article { Title = "Only this is set" };

        var doc = JsonDocument.Parse(Converter.WriteEnvelopes(article));

        doc.RootElement.GetArrayLength().Should().Be(1);
        doc.RootElement[0].GetProperty("element").GetProperty("codename").GetString().Should().Be("title");
        doc.RootElement[0].GetProperty("value").GetString().Should().Be("Only this is set");
    }

    [Fact]
    public void Text_WritesString()
    {
        var article = new Article { Title = "Hello", Abstract = "World" };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(article));

        envelopes["title"].GetProperty("value").GetString().Should().Be("Hello");
        envelopes["abstract"].GetProperty("value").GetString().Should().Be("World");
    }

    [Fact]
    public void Number_WritesDecimal()
    {
        var product = new Product { Price = 19.95m };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(product));

        envelopes["price"].GetProperty("value").GetDecimal().Should().Be(19.95m);
    }

    [Fact]
    public void DateTime_WritesIsoString()
    {
        var publishedAt = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero);
        var article = new Article { PublishingDate = publishedAt };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(article));

        envelopes["publishing_date"].GetProperty("value").GetDateTimeOffset().Should().Be(publishedAt);
    }

    [Fact]
    public void MultipleChoice_WritesCodenameArray()
    {
        var callout = new Callout { Type = [CalloutType.Warning, CalloutType.Info] };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(callout));

        var values = envelopes["type"].GetProperty("value");
        values.GetArrayLength().Should().Be(2);
        values[0].GetProperty("codename").GetString().Should().Be("warning");
        values[1].GetProperty("codename").GetString().Should().Be("info");
    }

    [Fact]
    public void Asset_WritesIdentifierArray()
    {
        var assetId = Guid.NewGuid();
        var article = new Article { HeroImage = [new AssetReference { Id = assetId }] };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(article));

        var values = envelopes["hero_image"].GetProperty("value");
        values.GetArrayLength().Should().Be(1);
        values[0].GetProperty("id").GetGuid().Should().Be(assetId);
        values[0].TryGetProperty("codename", out _).Should().BeFalse("nulls should be omitted");
    }

    [Fact]
    public void Reference_Taxonomy_WritesIdentifierArray()
    {
        var termId = Guid.NewGuid();
        var article = new Article { Type = [Reference.ById(termId)] };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(article));

        var values = envelopes["type"].GetProperty("value");
        values.GetArrayLength().Should().Be(1);
        values[0].GetProperty("id").GetGuid().Should().Be(termId);
    }

    [Fact]
    public void Reference_Subpages_WritesIdentifierArray()
    {
        var page = new Page { Subpages = [Reference.ByCodename("about"), Reference.ByExternalId("ext-1")] };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(page));

        var values = envelopes["subpages"].GetProperty("value");
        values.GetArrayLength().Should().Be(2);
        values[0].GetProperty("codename").GetString().Should().Be("about");
        values[1].GetProperty("external_id").GetString().Should().Be("ext-1");
    }

    [Fact]
    public void RichText_WithoutComponents_WritesValueOnly()
    {
        var person = new Person { Bio = new RichTextElement { Value = "<p>hello</p>" } };

        var envelopes = ParseEnvelopes(Converter.WriteEnvelopes(person));

        envelopes["bio"].GetProperty("value").GetString().Should().Be("<p>hello</p>");
        envelopes["bio"].TryGetProperty("components", out _).Should().BeFalse();
    }

    [Fact]
    public void EnvelopeShape_KeyedByCodename()
    {
        var article = new Article { Title = "x" };

        var doc = JsonDocument.Parse(Converter.WriteEnvelopes(article));

        var element = doc.RootElement[0].GetProperty("element");
        element.TryGetProperty("codename", out _).Should().BeTrue("MAPI accepts writes keyed by codename");
        element.TryGetProperty("id", out _).Should().BeFalse("writes should not include element id");
    }

    private static Dictionary<string, JsonElement> ParseEnvelopes(string json)
    {
        var doc = JsonDocument.Parse(json);
        return doc.RootElement.EnumerateArray()
            .ToDictionary(env => env.GetProperty("element").GetProperty("codename").GetString()!, env => env.Clone());
    }
}
