using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Modules.ModelBuilders;
using System.Text.Json;
// Models.Content also defines a RichTextElement; alias only the one symbol we need to avoid colliding with the
// element-builder RichTextElement under test.
using UrlSlugMode = Kontent.Ai.Management.Models.Content.UrlSlugMode;

namespace Kontent.Ai.Management.Tests.Modules.ModelBuilders;

// The typed element records serialize directly into LanguageVariantUpsertModel.Elements (an IEnumerable<object>), so the
// tests assert the wire each one produces through the client's real serializer options — references, enums, decimals and
// null-omission all come from there, the same path the client uses.
public class ElementBuilderTests
{
    private static readonly JsonSerializerOptions Options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

    // Serialize as object, mirroring how each element is serialized inside the IEnumerable<object> payload (runtime type).
    private static JsonElement Wire(BaseElement element)
        => JsonDocument.Parse(JsonSerializer.Serialize<object>(element, Options)).RootElement;

    [Fact]
    public void TextElement_SerializesValueAndElementReference()
    {
        var wire = Wire(new TextElement { Element = Reference.ByCodename("title"), Value = "Hello" });

        wire.GetProperty("value").GetString().Should().Be("Hello");
        wire.GetProperty("element").GetProperty("codename").GetString().Should().Be("title");
    }

    [Fact]
    public void NumberElement_SerializesDecimal()
    {
        var wire = Wire(new NumberElement { Element = Reference.ByCodename("price"), Value = 42.5m });

        wire.GetProperty("value").GetDecimal().Should().Be(42.5m);
    }

    [Fact]
    public void DateTimeElement_SerializesValueAsUtcZ_AndDisplayTimezone()
    {
        var wire = Wire(new DateTimeElement
        {
            Element = Reference.ByCodename("publishing_date"),
            Value = new DateTimeOffset(2024, 6, 1, 14, 0, 0, TimeSpan.FromHours(2)),
            DisplayTimeZone = "Europe/Prague",
        });

        // The +02:00 offset collapses to the UTC instant with a trailing Z, matching the API's storage.
        wire.GetProperty("value").GetString().Should().Be("2024-06-01T12:00:00Z");
        wire.GetProperty("display_timezone").GetString().Should().Be("Europe/Prague");
    }

    [Fact]
    public void DateTimeElement_WithoutZone_OmitsDisplayTimezone()
    {
        var wire = Wire(new DateTimeElement
        {
            Element = Reference.ByCodename("publishing_date"),
            Value = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
        });

        wire.TryGetProperty("display_timezone", out _).Should().BeFalse();
    }

    [Fact]
    public void MultipleChoiceElement_SerializesOptionReferences()
    {
        var wire = Wire(new MultipleChoiceElement
        {
            Element = Reference.ByCodename("type"),
            Value = [Reference.ByCodename("warning"), Reference.ByCodename("info")],
        });

        wire.GetProperty("value").EnumerateArray()
            .Select(o => o.GetProperty("codename").GetString())
            .Should().Equal("warning", "info");
    }

    [Fact]
    public void AssetElement_SerializesAssetReferences()
    {
        var id = Guid.NewGuid();
        var wire = Wire(new AssetElement
        {
            Element = Reference.ByCodename("hero"),
            Value = [new AssetWithRenditionsReference(Reference.ById(id))],
        });

        wire.GetProperty("value").EnumerateArray().Single()
            .GetProperty("id").GetGuid().Should().Be(id);
    }

    [Fact]
    public void UrlSlugElement_SerializesValueAndMode()
    {
        var wire = Wire(new UrlSlugElement
        {
            Element = Reference.ByCodename("slug"),
            Value = "on-roasts",
            Mode = UrlSlugMode.Custom,
        });

        wire.GetProperty("value").GetString().Should().Be("on-roasts");
        wire.GetProperty("mode").GetString().Should().Be("custom");
    }

    [Fact]
    public void CustomElement_SerializesValueAndSearchableValue()
    {
        var wire = Wire(new CustomElement
        {
            Element = Reference.ByCodename("rating"),
            Value = "{\"stars\":5}",
            SearchableValue = "five stars",
        });

        wire.GetProperty("value").GetString().Should().Be("{\"stars\":5}");
        wire.GetProperty("searchable_value").GetString().Should().Be("five stars");
    }

    [Fact]
    public void CustomElement_WithoutSearchableValue_OmitsIt()
    {
        var wire = Wire(new CustomElement { Element = Reference.ByCodename("rating"), Value = "{}" });

        wire.TryGetProperty("searchable_value", out _).Should().BeFalse();
    }

    [Fact]
    public void RichTextElement_SerializesValueAndComponents()
    {
        var componentId = Guid.NewGuid();
        var wire = Wire(new RichTextElement
        {
            Element = Reference.ByCodename("body"),
            Value = "<p>hello</p>",
            Components =
            [
                new ComponentModel { Id = componentId, Type = Reference.ByCodename("callout"), Elements = [] },
            ],
        });

        wire.GetProperty("value").GetString().Should().Be("<p>hello</p>");
        wire.GetProperty("components").EnumerateArray().Single()
            .GetProperty("id").GetGuid().Should().Be(componentId);
    }

    [Fact]
    public void LinkedItemsElement_SerializesItemReferences()
    {
        var wire = Wire(new LinkedItemsElement
        {
            Element = Reference.ByCodename("related"),
            Value = [Reference.ByCodename("article_a")],
        });

        wire.GetProperty("value").EnumerateArray().Single()
            .GetProperty("codename").GetString().Should().Be("article_a");
    }

    [Fact]
    public void GetElements_BuildsUpsertPayload_WithEachElementSerialized()
    {
        var model = new LanguageVariantUpsertModel
        {
            Elements = ElementBuilder.GetElements(
                new TextElement { Element = Reference.ByCodename("title"), Value = "On Roasts" },
                new DateTimeElement { Element = Reference.ByCodename("publishing_date"), Value = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero) }),
        };

        var elements = JsonDocument.Parse(JsonSerializer.Serialize(model, Options)).RootElement
            .GetProperty("elements").EnumerateArray()
            .ToDictionary(e => e.GetProperty("element").GetProperty("codename").GetString()!, e => e);

        elements["title"].GetProperty("value").GetString().Should().Be("On Roasts");
        elements["publishing_date"].GetProperty("value").GetString().Should().Be("2024-06-01T12:00:00Z");
    }

    [Fact]
    public void GetElements_NullArray_Throws()
    {
        var act = () => ElementBuilder.GetElements(null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }
}
