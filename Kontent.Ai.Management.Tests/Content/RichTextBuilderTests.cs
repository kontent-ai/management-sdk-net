using AwesomeAssertions;
using Kontent.Ai.Management.Models.Content;
using MyProject.Models;
using System.Text.RegularExpressions;
// MyProject.Models.Action (a content-type fixture) shadows System.Action — alias to avoid the ambiguity in lambdas.
using SystemAction = System.Action;

namespace Kontent.Ai.Management.Tests.Content;

public class RichTextBuilderTests
{
    // --- Component ---

    [Fact]
    public void Component_GuidInMarkupMatchesRecordedComponentId()
    {
        var rt = new RichTextBuilder();
        var callout = new Callout { Type = [CalloutType.Warning] };

        var placeholder = rt.Component(callout);
        var element = rt.Build($"<p>{placeholder}</p>");

        element.Components.Should().ContainSingle();
        var component = element.Components![0];
        component.Content.Should().BeSameAs(callout);

        var idInMarkup = ExtractAttribute(placeholder, "data-id");
        Guid.Parse(idInMarkup).Should().Be(component.Id);
    }

    [Fact]
    public void Component_MultipleCalls_MintDistinctGuids()
    {
        var rt = new RichTextBuilder();

        var first = rt.Component(new Callout { Type = [CalloutType.Info] });
        var second = rt.Component(new Callout { Type = [CalloutType.Warning] });

        ExtractAttribute(first, "data-id").Should().NotBe(ExtractAttribute(second, "data-id"));
        var element = rt.Build($"{first}{second}");
        element.Components.Should().HaveCount(2);
        element.Components![0].Id.Should().NotBe(element.Components[1].Id);
    }

    [Fact]
    public void Component_PreservesInterpolationOrder()
    {
        // Records the components in the order the helper is called inside an interpolated string.
        var rt = new RichTextBuilder();
        var first = new Callout { Type = [CalloutType.Info] };
        var second = new Callout { Type = [CalloutType.Warning] };

        var element = rt.Build($"<p>{rt.Component(first)}{rt.Component(second)}</p>");

        element.Components.Should().HaveCount(2);
        element.Components![0].Content.Should().BeSameAs(first);
        element.Components[1].Content.Should().BeSameAs(second);
    }

    [Fact]
    public void Component_NullItem_Throws() =>
        new SystemAction(() => new RichTextBuilder().Component(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void Component_ItemWithoutKontentType_Throws()
    {
        var rt = new RichTextBuilder();

        var act = () => rt.Component(new BareContentItem());

        act.Should().Throw<InvalidOperationException>().WithMessage("*KontentType*");
    }

    // --- LinkedItem ---

    [Fact]
    public void LinkedItem_ById_EmitsDataIdAttribute()
    {
        var id = Guid.NewGuid();

        var markup = new RichTextBuilder().LinkedItem(Reference.ById(id));

        markup.Should().Contain($"""data-type="item" data-id="{id}" """.TrimEnd());
    }

    [Fact]
    public void LinkedItem_ByCodename_EmitsDataCodenameAttribute()
    {
        var markup = new RichTextBuilder().LinkedItem(Reference.ByCodename("intro_article"));

        markup.Should().Contain("""data-codename="intro_article" """.TrimEnd());
    }

    [Fact]
    public void LinkedItem_ByExternalId_EmitsDataExternalIdAttribute()
    {
        var markup = new RichTextBuilder().LinkedItem(Reference.ByExternalId("ext-123"));

        markup.Should().Contain("""data-external-id="ext-123" """.TrimEnd());
    }

    // --- ItemLink ---

    [Fact]
    public void ItemLink_ById_EmitsDataItemIdAttribute()
    {
        var id = Guid.NewGuid();

        var markup = new RichTextBuilder().ItemLink(Reference.ById(id), "the article");

        markup.Should().Be($"""<a data-item-id="{id}">the article</a>""");
    }

    [Fact]
    public void ItemLink_ByCodename_EmitsDataItemCodenameAttribute()
    {
        var markup = new RichTextBuilder().ItemLink(Reference.ByCodename("intro"), "intro");

        markup.Should().Contain("""data-item-codename="intro" """.TrimEnd());
    }

    [Fact]
    public void ItemLink_ByExternalId_EmitsDataItemExternalIdAttribute()
    {
        var markup = new RichTextBuilder().ItemLink(Reference.ByExternalId("ext-1"), "see");

        markup.Should().Contain("""data-item-external-id="ext-1" """.TrimEnd());
    }

    [Fact]
    public void ItemLink_LinkText_IsHtmlEncoded()
    {
        // Special chars in the link text must be encoded so the resulting HTML is well-formed.
        var markup = new RichTextBuilder().ItemLink(Reference.ByCodename("x"), "AT&T <Spec>");

        markup.Should().Contain("AT&amp;T &lt;Spec&gt;");
        markup.Should().NotContain("AT&T <Spec>");
    }

    [Fact]
    public void ItemLink_NullLinkText_Throws() =>
        new SystemAction(() => new RichTextBuilder().ItemLink(Reference.ByCodename("x"), null!))
            .Should().Throw<ArgumentNullException>();

    // --- Asset ---

    [Fact]
    public void Asset_ById_EmitsDataAssetIdAttribute()
    {
        var id = Guid.NewGuid();

        var markup = new RichTextBuilder().Asset(new AssetReference { Id = id });

        markup.Should().Be($"""<figure data-asset-id="{id}"></figure>""");
    }

    [Fact]
    public void Asset_ByCodename_EmitsDataAssetCodenameAttribute()
    {
        var markup = new RichTextBuilder().Asset(new AssetReference { Codename = "hero" });

        markup.Should().Contain("""data-asset-codename="hero" """.TrimEnd());
    }

    [Fact]
    public void Asset_ByExternalId_EmitsDataAssetExternalIdAttribute()
    {
        var markup = new RichTextBuilder().Asset(new AssetReference { ExternalId = "ext-img" });

        markup.Should().Contain("""data-asset-external-id="ext-img" """.TrimEnd());
    }

    [Fact]
    public void Asset_EmptyReference_Throws()
    {
        var act = () => new RichTextBuilder().Asset(new AssetReference());

        act.Should().Throw<ArgumentException>();
    }

    // --- Build ---

    [Fact]
    public void Build_NoComponents_ComponentsIsNull()
    {
        var element = new RichTextBuilder().Build("<p>plain</p>");

        element.Value.Should().Be("<p>plain</p>");
        element.Components.Should().BeNull();
    }

    [Fact]
    public void Build_VerbatimHtml_IsNotMutated()
    {
        var html = "<p>foo &amp; bar</p>";

        var element = new RichTextBuilder().Build(html);

        element.Value.Should().Be(html);
    }

    [Fact]
    public void Build_NullHtml_Throws() =>
        new SystemAction(() => new RichTextBuilder().Build(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void Build_IsReusable_AndClearsRecordedComponents()
    {
        // After Build, the builder starts fresh — components from the previous Build don't leak into the next.
        var rt = new RichTextBuilder();
        rt.Component(new Callout { Type = [CalloutType.Info] });
        var first = rt.Build("<p>first</p>");

        var second = rt.Build("<p>second</p>");

        first.Components.Should().ContainSingle();
        second.Components.Should().BeNull();
    }

    // --- Nesting ---

    [Fact]
    public void NestedBuilders_AreIndependent()
    {
        // A component whose own Content is a RichTextValue built by a nested RichTextBuilder must not leak its
        // inner components onto the outer builder.
        var outer = new RichTextBuilder();
        var inner = new RichTextBuilder();

        var nested = new Callout
        {
            Content = inner.Build($"<p>{inner.Component(new Callout { Type = [CalloutType.Warning] })}</p>"),
        };
        var element = outer.Build($"<p>{outer.Component(nested)}</p>");

        element.Components.Should().ContainSingle();
        var outerComponent = element.Components![0];
        outerComponent.Content.Should().BeSameAs(nested);
        // The outer's recorded component does NOT include the inner builder's component as a sibling —
        // the warning callout lives one level down, inside the outer component's own rich-text content.
        nested.Content!.Components.Should().ContainSingle()
            .Which.Content.Should().BeOfType<Callout>()
            .Which.Type.Should().Equal(CalloutType.Warning);
    }

    // --- Helpers ---

    private static string ExtractAttribute(string markup, string attribute)
    {
        var match = Regex.Match(markup, $@"{attribute}=""(?<v>[^""]+)""");
        match.Success.Should().BeTrue($"markup '{markup}' should carry attribute '{attribute}'");
        return match.Groups["v"].Value;
    }

    private sealed record BareContentItem : IElementsModel;
}
