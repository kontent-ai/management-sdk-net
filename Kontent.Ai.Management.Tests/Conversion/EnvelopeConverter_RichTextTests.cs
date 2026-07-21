using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Models.Content;
using MyProject.Models;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Conversion;

public class EnvelopeConverter_RichTextTests
{
    private static readonly ContentItemEnvelopeConverter Converter = CreateConverter();
    private static readonly Guid SampleComponentId = new("44444444-4444-4444-4444-444444444444");

    private static ContentItemEnvelopeConverter CreateConverter()
    {
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(Callout));
        return new ContentItemEnvelopeConverter(registry);
    }

    [Fact]
    public void Write_RichTextWithComponent_EmitsTypeAndElementsArray()
    {
        var callout = new Callout
        {
            Content = new RichTextValue { Value = "<p>watch out</p>" },
            Type = [CalloutType.Warning],
        };
        var article = new Article
        {
            Content = new RichTextValue
            {
                Value = "<p>intro</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"...\"></object>",
                Components = [new Component { Id = SampleComponentId, Content = callout }],
            },
        };

        var doc = JsonDocument.Parse(Converter.WriteEnvelopes(article));

        var contentEnvelope = doc.RootElement.EnumerateArray()
            .Single(e => e.GetProperty("element").GetProperty("codename").GetString() == "content");
        var components = contentEnvelope.GetProperty("components");
        components.GetArrayLength().Should().Be(1);

        var component = components[0];
        component.GetProperty("id").GetGuid().Should().Be(SampleComponentId);
        component.GetProperty("type").GetProperty("codename").GetString().Should().Be("callout");

        var inner = component.GetProperty("elements");
        var innerCodenames = inner.EnumerateArray()
            .Select(env => env.GetProperty("element").GetProperty("codename").GetString())
            .ToHashSet();
        innerCodenames.Should().BeEquivalentTo("content", "type");
    }

    [Fact]
    public void Read_RichTextWithComponent_MaterializesNestedRecord()
    {
        var json = """
            [{
              "element": { "id": "7d4b95b0-76fc-552e-8931-c90dd2746399" },
              "value": "<p>body</p>",
              "components": [{
                "id": "44444444-4444-4444-4444-444444444444",
                "type": { "id": "72c5b04f-e316-5912-baf2-8ccd2f0ad7a2" },
                "elements": [
                  { "element": { "id": "15a9fc79-e85f-5bb1-8e81-d0362cd93b93" }, "value": "<p>nested</p>" },
                  { "element": { "id": "44dd9032-c950-53b8-91bd-c6c586233311" }, "value": [{ "id": "e0b420c5-e95f-5a51-83db-39627cecc00b" }] }
                ]
              }]
            }]
            """;

        var article = Converter.ReadEnvelopes<Article>(json);

        article.Content!.Value.Should().Be("<p>body</p>");
        var component = article.Content.Components.Should().ContainSingle().Subject;
        component.Id.Should().Be(SampleComponentId);
        var nestedCallout = component.Content.Should().BeOfType<Callout>().Subject;
        nestedCallout.Content!.Value.Should().Be("<p>nested</p>");
        nestedCallout.Type.Should().Equal(CalloutType.Lightbulb);
    }

    [Fact]
    public void Read_ComponentWithUnregisteredId_Throws()
    {
        // Registry knows `article` but not the synthetic component type id below.
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(Article));
        var converter = new ContentItemEnvelopeConverter(registry);

        var json = """
            [{
              "element": { "id": "7d4b95b0-76fc-552e-8931-c90dd2746399" },
              "value": "<p>x</p>",
              "components": [{
                "id": "44444444-4444-4444-4444-444444444444",
                "type": { "id": "99999999-9999-9999-9999-999999999999" },
                "elements": []
              }]
            }]
            """;

        var act = () => converter.ReadEnvelopes<Article>(json);

        act.Should().Throw<InvalidOperationException>().WithMessage("*99999999-9999-9999-9999-999999999999*");
    }

    [Fact]
    public void Write_ComponentWithoutKontentTypeAttribute_Throws()
    {
        var article = new Article
        {
            Content = new RichTextValue
            {
                Value = "<p>x</p>",
                Components = [new Component { Id = SampleComponentId, Content = new BareContentItem() }],
            },
        };

        var act = () => Converter.WriteEnvelopes(article);

        act.Should().Throw<InvalidOperationException>().WithMessage("*KontentType*");
    }

    // A degenerate IElementsModel that lacks [KontentType] — exercises the converter's defensive check.
    private sealed record BareContentItem : IElementsModel;
}
