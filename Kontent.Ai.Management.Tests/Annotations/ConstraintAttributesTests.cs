using AwesomeAssertions;
using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Annotations;

public class ConstraintAttributesTests
{
    [Fact]
    public void AllowedTypes_StoresCodenamesInOrder()
    {
        var attr = new AllowedTypesAttribute("article", "page", "snippet");

        attr.Codenames.Should().Equal("article", "page", "snippet");
    }

    [Fact]
    public void AllowedTypes_EmptyParams_AllowsEmptyList()
    {
        var attr = new AllowedTypesAttribute();

        attr.Codenames.Should().BeEmpty();
    }

    [Fact]
    public void AllowedTypes_NullCodenames_Throws()
    {
        Action act = () => _ = new AllowedTypesAttribute(null!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("codenames");
    }

    [Fact]
    public void AllowedItemLinkTypes_StoresCodenames()
    {
        var attr = new AllowedItemLinkTypesAttribute("article", "page");

        attr.Codenames.Should().Equal("article", "page");
    }

    [Fact]
    public void AllowedItemLinkTypes_NullCodenames_Throws()
    {
        Action act = () => _ = new AllowedItemLinkTypesAttribute(null!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("codenames");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public void MinElements_StoresN(int n)
    {
        new MinElementsAttribute(n).N.Should().Be(n);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    public void MaxElements_StoresN(int n)
    {
        new MaxElementsAttribute(n).N.Should().Be(n);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void ExactElements_StoresN(int n)
    {
        new ExactElementsAttribute(n).N.Should().Be(n);
    }

    [Fact]
    public void MaxAssetSize_StoresBytes()
    {
        new MaxAssetSizeAttribute(1_048_576).Bytes.Should().Be(1_048_576);
    }

    [Theory]
    [InlineData(AssetFileType.Any)]
    [InlineData(AssetFileType.Adjustable)]
    [InlineData(AssetFileType.Image)]
    public void AllowedAssetFileTypes_StoresEnum(AssetFileType type)
    {
        new AllowedAssetFileTypesAttribute(type).Types.Should().Be(type);
    }

    [Fact]
    public void AssetFileType_Default_IsAny()
    {
        // The default enum value matters — generator emits no attribute when the constraint is Any,
        // so a default-constructed AssetFileType must mean "no restriction."
        default(AssetFileType).Should().Be(AssetFileType.Any);
    }

    [Fact]
    public void AllowedTaxonomyGroup_StoresValue()
    {
        new AllowedTaxonomyGroupAttribute("categories").CodenameOrId.Should().Be("categories");
        new AllowedTaxonomyGroupAttribute("77777777-7777-7777-7777-777777777777").CodenameOrId
            .Should().Be("77777777-7777-7777-7777-777777777777");
    }

    [Fact]
    public void AllowedTaxonomyGroup_Null_Throws()
    {
        Action act = () => _ = new AllowedTaxonomyGroupAttribute(null!);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("codenameOrId");
    }

    [Theory]
    [InlineData(typeof(AllowedTypesAttribute), AttributeTargets.Property)]
    [InlineData(typeof(AllowedItemLinkTypesAttribute), AttributeTargets.Property)]
    [InlineData(typeof(MinElementsAttribute), AttributeTargets.Property)]
    [InlineData(typeof(MaxElementsAttribute), AttributeTargets.Property)]
    [InlineData(typeof(ExactElementsAttribute), AttributeTargets.Property)]
    [InlineData(typeof(MaxAssetSizeAttribute), AttributeTargets.Property)]
    [InlineData(typeof(AllowedAssetFileTypesAttribute), AttributeTargets.Property)]
    [InlineData(typeof(AllowedTaxonomyGroupAttribute), AttributeTargets.Property)]
    public void ConstraintAttributes_TargetPropertyOnly(Type attrType, AttributeTargets expectedTargets)
    {
        var usage = attrType.GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>().Single();

        usage.ValidOn.Should().Be(expectedTargets);
        usage.AllowMultiple.Should().BeFalse();
    }
}
