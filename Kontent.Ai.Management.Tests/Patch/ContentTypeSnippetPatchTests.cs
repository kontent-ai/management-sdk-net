using AwesomeAssertions;
using Kontent.Ai.Management.Models.ContentModel.Patch;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.TypeSnippets.Patch;

namespace Kontent.Ai.Management.Tests.Patch;

public class ContentTypeSnippetPatchTests
{
    private static readonly Reference El = Reference.ByCodename("body");

    [Fact]
    public void AddElement_AppendsToElements()
    {
        var op = (ContentModelAddIntoPatchModel)ContentTypeSnippetPatch.AddElement(new TextElementMetadataModel { Name = "Title" });

        op.Op.Should().Be("addInto");
        op.Path.Should().Be("/elements");
    }

    [Fact]
    public void ReplaceGuidelines_TargetsGuidelines()
    {
        var op = (ContentModelReplacePatchModel)ContentTypeSnippetPatch.ReplaceGuidelines(El, "text");

        op.Op.Should().Be("replace");
        op.Path.Should().Be("/elements/codename:body/guidelines");
        op.Value.Should().Be("text");
    }

    [Fact]
    public void AllowedItemLinkTypes_ReplaceWholeArray()
    {
        var types = new[] { Reference.ByCodename("article") };
        var op = (ContentModelReplacePatchModel)ContentTypeSnippetPatch.ReplaceAllowedItemLinkTypes(El, types);

        op.Path.Should().Be("/elements/codename:body/allowed_item_link_types");
        op.Value.Should().BeSameAs(types);
    }

    [Fact]
    public void AllowedFormatting_AddUsesWireToken()
    {
        var op = (ContentModelAddIntoPatchModel)ContentTypeSnippetPatch.AddAllowedFormatting(El, RichTextFormattingType.Unstyled);

        op.Path.Should().Be("/elements/codename:body/allowed_formatting");
        op.Value.Should().Be("unstyled");
    }

    [Fact]
    public void RemoveRaw_PassesPathThrough()
    {
        var op = (ContentModelRemovePatchModel)ContentTypeSnippetPatch.RemoveRaw("/elements/codename:body/whatever");

        op.Path.Should().Be("/elements/codename:body/whatever");
    }
}
