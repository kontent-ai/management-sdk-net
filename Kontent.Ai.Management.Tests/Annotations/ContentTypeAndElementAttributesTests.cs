using System.Reflection;
using FluentAssertions;
using Kontent.Ai.Management.Annotations;
using Xunit;

namespace Kontent.Ai.Management.Tests.Annotations;

public class ContentTypeAndElementAttributesTests
{
    [Fact]
    public void KontentContentType_StoresCodenameAndOptionalId()
    {
        var withId = new KontentTypeAttribute("article", "11111111-1111-1111-1111-111111111111");
        var withoutId = new KontentTypeAttribute("article");

        withId.Codename.Should().Be("article");
        withId.Id.Should().Be("11111111-1111-1111-1111-111111111111");

        withoutId.Codename.Should().Be("article");
        withoutId.Id.Should().BeNull();
    }

    [Fact]
    public void KontentContentType_NullCodename_Throws()
    {
        Action act = () => _ = new KontentTypeAttribute(null!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("codename");
    }

    [Fact]
    public void KontentContentType_UsageTargetsClassOnly()
    {
        var usage = typeof(KontentTypeAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>().Single();

        usage.ValidOn.Should().Be(AttributeTargets.Class);
        usage.AllowMultiple.Should().BeFalse();
        usage.Inherited.Should().BeFalse();
    }

    [Fact]
    public void KontentElement_StoresCodenameAndId()
    {
        var attr = new KontentElementAttribute("title", "22222222-2222-2222-2222-222222222222");

        attr.Codename.Should().Be("title");
        attr.Id.Should().Be("22222222-2222-2222-2222-222222222222");
    }

    [Theory]
    [InlineData(null, "id", "codename")]
    [InlineData("codename", null, "id")]
    public void KontentElement_NullArgs_Throw(string? codename, string? id, string expectedParam)
    {
        Action act = () => _ = new KontentElementAttribute(codename!, id!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be(expectedParam);
    }

    [Fact]
    public void KontentElement_UsageTargetsPropertyOnly()
    {
        var usage = typeof(KontentElementAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>().Single();

        usage.ValidOn.Should().Be(AttributeTargets.Property);
        usage.AllowMultiple.Should().BeFalse();
    }

    [Fact]
    public void KontentEnumValue_StoresCodenameAndId()
    {
        var attr = new KontentEnumValueAttribute("news", "33333333-3333-3333-3333-333333333333");

        attr.Codename.Should().Be("news");
        attr.Id.Should().Be("33333333-3333-3333-3333-333333333333");
    }

    [Theory]
    [InlineData(null, "id", "codename")]
    [InlineData("codename", null, "id")]
    public void KontentEnumValue_NullArgs_Throw(string? codename, string? id, string expectedParam)
    {
        Action act = () => _ = new KontentEnumValueAttribute(codename!, id!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be(expectedParam);
    }

    [Fact]
    public void KontentEnumValue_UsageTargetsFieldOnly()
    {
        var usage = typeof(KontentEnumValueAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>().Single();

        usage.ValidOn.Should().Be(AttributeTargets.Field);
        usage.AllowMultiple.Should().BeFalse();
    }

    // End-to-end: applying the attributes to a stub generated record reads back the same values via reflection.
    [KontentType("article", "44444444-4444-4444-4444-444444444444")]
    private sealed class StubArticle
    {
        [KontentElement("title", "55555555-5555-5555-5555-555555555555")]
        public string? Title { get; init; }
    }

    private enum StubCategory
    {
        [KontentEnumValue("news", "66666666-6666-6666-6666-666666666666")] News,
    }

    [Fact]
    public void Attributes_RoundTripThroughReflection()
    {
        var type = typeof(StubArticle).GetCustomAttribute<KontentTypeAttribute>()!;
        type.Codename.Should().Be("article");

        var prop = typeof(StubArticle).GetProperty(nameof(StubArticle.Title))!
            .GetCustomAttribute<KontentElementAttribute>()!;
        prop.Codename.Should().Be("title");
        prop.Id.Should().Be("55555555-5555-5555-5555-555555555555");

        var enumValue = typeof(StubCategory).GetField(nameof(StubCategory.News))!
            .GetCustomAttribute<KontentEnumValueAttribute>()!;
        enumValue.Codename.Should().Be("news");
    }
}
