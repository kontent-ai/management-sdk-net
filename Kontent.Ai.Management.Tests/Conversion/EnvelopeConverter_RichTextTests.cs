using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Models.Content;
using MyProject.Models;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Conversion;

public class EnvelopeConverter_RichTextTests
{
    // Component candidates must be pre-registered so the read path can dispatch by content-type codename.
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
        innerCodenames.Should().BeEquivalentTo(["content", "type"]);
    }

    [Fact]
    public void RoundTrip_RichTextWithComponent_PreservesAll()
    {
        var original = new Article
        {
            Title = "Roundtrip",
            Content = new RichTextValue
            {
                Value = "<p>body</p>",
                Components =
                [
                    new Component
                    {
                        Id = SampleComponentId,
                        Content = new Callout
                        {
                            Type = [CalloutType.Lightbulb],
                            Content = new RichTextValue { Value = "<p>nested</p>" },
                        },
                    },
                ],
            },
        };

        var wire = Converter.WriteEnvelopes(original);
        var roundtripped = Converter.ReadEnvelopes<Article>(wire);

        roundtripped.Title.Should().Be("Roundtrip");
        roundtripped.Content!.Value.Should().Be("<p>body</p>");
        roundtripped.Content.Components.Should().ContainSingle();

        var component = roundtripped.Content.Components!.ElementAt(0);
        component.Id.Should().Be(SampleComponentId);
        var nestedCallout = component.Content.Should().BeOfType<Callout>().Subject;
        nestedCallout.Type.Should().Equal(CalloutType.Lightbulb);
        nestedCallout.Content!.Value.Should().Be("<p>nested</p>");
    }

    [Fact]
    public void Read_ComponentWithUnregisteredCodename_Throws()
    {
        // Use a registry that has been scanned to know `article` but not the synthetic codename below.
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(Article));
        var converter = new ContentItemEnvelopeConverter(registry);

        var json = """
            [{
              "element": { "id": "7d4b95b0-76fc-552e-8931-c90dd2746399" },
              "value": "<p>x</p>",
              "components": [{
                "id": "44444444-4444-4444-4444-444444444444",
                "type": { "codename": "totally_unknown_type" },
                "elements": []
              }]
            }]
            """;

        var act = () => converter.ReadEnvelopes<Article>(json);

        act.Should().Throw<InvalidOperationException>().WithMessage("*totally_unknown_type*");
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
