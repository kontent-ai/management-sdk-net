using AwesomeAssertions;
using Kontent.Ai.Management.Models.ContentModel.Patch;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Patch;

namespace Kontent.Ai.Management.Tests.Patch;

public class ContentTypePatchTests
{
    private static readonly Reference El = Reference.ByCodename("title");

    [Fact]
    public void AddElement_AppendsToElements()
    {
        var op = (ContentModelAddIntoPatchModel)ContentTypePatch.AddElement(new TextElementMetadataModel { Name = "Title" });

        op.Op.Should().Be("addInto");
        op.Path.Should().Be("/elements");
        op.Value.Should().BeOfType<TextElementMetadataModel>();
        op.Before.Should().BeNull();
        op.After.Should().BeNull();
    }

    [Fact]
    public void AddElement_WithBoth_BeforeAndAfter_Throws()
    {
        var act = () => ContentTypePatch.AddElement(
            new TextElementMetadataModel { Name = "Title" }, before: El, after: Reference.ByCodename("body"));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RemoveElement_TargetsElementBySelector()
    {
        var op = (ContentModelRemovePatchModel)ContentTypePatch.RemoveElement(El);

        op.Op.Should().Be("remove");
        op.Path.Should().Be("/elements/codename:title");
    }

    [Fact]
    public void MoveElementBefore_SetsBeforeOnly()
    {
        var target = Reference.ByCodename("body");
        var op = (ContentModelMovePatchModel)ContentTypePatch.MoveElementBefore(El, target);

        op.Op.Should().Be("move");
        op.Path.Should().Be("/elements/codename:title");
        op.Before.Should().Be(target);
        op.After.Should().BeNull();
    }

    [Theory]
    [InlineData("title", "/elements/codename:title/guidelines")]
    public void ReplaceGuidelines_TargetsGuidelinesProperty(string codename, string expectedPath)
    {
        var op = (ContentModelReplacePatchModel)ContentTypePatch.ReplaceGuidelines(Reference.ByCodename(codename), "Keep it short.");

        op.Op.Should().Be("replace");
        op.Path.Should().Be(expectedPath);
        op.Value.Should().Be("Keep it short.");
    }

    [Fact]
    public void ReplaceGuidelines_Null_Clears()
    {
        var op = (ContentModelReplacePatchModel)ContentTypePatch.ReplaceGuidelines(El, null);

        op.Value.Should().BeNull();
    }

    [Fact]
    public void ReplaceIsRequired_EmitsBoolValue()
    {
        var op = (ContentModelReplacePatchModel)ContentTypePatch.ReplaceIsRequired(El, true);

        op.Path.Should().Be("/elements/codename:title/is_required");
        op.Value.Should().Be(true);
    }

    [Fact]
    public void ReplaceContentGroup_TargetsContentGroupProperty()
    {
        var group = Reference.ByCodename("group_b");
        var op = (ContentModelReplacePatchModel)ContentTypePatch.ReplaceContentGroup(El, group);

        op.Path.Should().Be("/elements/codename:title/content_group");
        op.Value.Should().Be(group);
    }

    [Fact]
    public void AllowedContentTypes_AllThreeIdioms()
    {
        var type = Reference.ByCodename("article");

        var add = (ContentModelAddIntoPatchModel)ContentTypePatch.AddAllowedContentType(El, type);
        add.Path.Should().Be("/elements/codename:title/allowed_content_types");
        add.Value.Should().Be(type);

        var remove = (ContentModelRemovePatchModel)ContentTypePatch.RemoveAllowedContentType(El, type);
        remove.Path.Should().Be("/elements/codename:title/allowed_content_types/codename:article");

        var types = new[] { type, Reference.ByCodename("blog_post") };
        var replace = (ContentModelReplacePatchModel)ContentTypePatch.ReplaceAllowedContentTypes(El, types);
        replace.Path.Should().Be("/elements/codename:title/allowed_content_types");
        replace.Value.Should().BeSameAs(types);
    }

    [Fact]
    public void Options_AddRemoveMove()
    {
        var option = Reference.ByCodename("news");

        var add = (ContentModelAddIntoPatchModel)ContentTypePatch.AddOption(El, new MultipleChoiceOptionModel { Name = "News" });
        add.Path.Should().Be("/elements/codename:title/options");

        var remove = (ContentModelRemovePatchModel)ContentTypePatch.RemoveOption(El, option);
        remove.Path.Should().Be("/elements/codename:title/options/codename:news");

        var move = (ContentModelMovePatchModel)ContentTypePatch.MoveOptionAfter(El, option, Reference.ByCodename("release"));
        move.Path.Should().Be("/elements/codename:title/options/codename:news");
        move.After.Should().Be(Reference.ByCodename("release"));
    }

    [Fact]
    public void ContentGroups_AddRemove()
    {
        var add = (ContentModelAddIntoPatchModel)ContentTypePatch.AddContentGroup(new ContentGroupModel { Name = "Group A" });
        add.Path.Should().Be("/content_groups");

        var remove = (ContentModelRemovePatchModel)ContentTypePatch.RemoveContentGroup(Reference.ByCodename("group_a"));
        remove.Path.Should().Be("/content_groups/codename:group_a");
    }

    [Fact]
    public void AllowedBlock_AddUsesWireToken_RemovePutsTokenInPath()
    {
        var add = (ContentModelAddIntoPatchModel)ContentTypePatch.AddAllowedBlock(El, RichTextBlockType.ComponentsAndItems);
        add.Path.Should().Be("/elements/codename:title/allowed_blocks");
        add.Value.Should().Be("components-and-items");

        var remove = (ContentModelRemovePatchModel)ContentTypePatch.RemoveAllowedBlock(El, RichTextBlockType.ComponentsAndItems);
        remove.Path.Should().Be("/elements/codename:title/allowed_blocks/components-and-items");
    }

    [Fact]
    public void AllowedTableTextBlock_UsesTableTextBlocksPathAndToken()
    {
        var remove = (ContentModelRemovePatchModel)ContentTypePatch.RemoveAllowedTableTextBlock(El, RichTextTextBlockType.HeadingThree);

        remove.Path.Should().Be("/elements/codename:title/allowed_table_text_blocks/heading-three");
    }

    [Theory]
    [InlineData("id")]
    public void Selector_RendersEachReferenceKind(string _)
    {
        ((ContentModelRemovePatchModel)ContentTypePatch.RemoveElement(Reference.ById(Guid.Empty)))
            .Path.Should().Be("/elements/id:00000000-0000-0000-0000-000000000000");
        ((ContentModelRemovePatchModel)ContentTypePatch.RemoveElement(Reference.ByCodename("my_field")))
            .Path.Should().Be("/elements/codename:my_field");
        ((ContentModelRemovePatchModel)ContentTypePatch.RemoveElement(Reference.ByExternalId("ext-1")))
            .Path.Should().Be("/elements/external_id:ext-1");
    }

    [Fact]
    public void Selector_EscapesForwardSlashInExternalId()
    {
        var op = (ContentModelRemovePatchModel)ContentTypePatch.RemoveElement(Reference.ByExternalId("my/value"));

        op.Path.Should().Be("/elements/external_id:my\\/value");
    }

    [Fact]
    public void Raw_EscapeHatch_PassesPathThrough()
    {
        var op = (ContentModelReplacePatchModel)ContentTypePatch.ReplaceRaw("/elements/codename:x/some_future_prop", 42);

        op.Path.Should().Be("/elements/codename:x/some_future_prop");
        op.Value.Should().Be(42);
    }

    [Fact]
    public void Raw_NullOrEmptyPath_Throws()
    {
        ((System.Action)(() => ContentTypePatch.ReplaceRaw(null!, 1))).Should().Throw<ArgumentException>();
        ((System.Action)(() => ContentTypePatch.RemoveRaw(""))).Should().Throw<ArgumentException>();
        ((System.Action)(() => ContentTypePatch.AddIntoRaw("", new object()))).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ReplaceName_NullName_Throws() =>
        ((System.Action)(() => ContentTypePatch.ReplaceName(null!))).Should().Throw<ArgumentNullException>();
}
