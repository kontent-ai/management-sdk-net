using Kontent.Ai.Management.Models.ContentModel.Patch;

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Intent-revealing factories for content-type PATCH operations, ready to pass to <c>ModifyContentTypeAsync</c>.
/// Inherits every shared operation from <see cref="ContentModelPatchBase"/> and adds the content-group operations,
/// which apply to content types only (snippets have none).
/// </summary>
public sealed class ContentTypePatch : ContentModelPatchBase
{
    private ContentTypePatch() { }

    // ---- Content groups (content types only) ----

    /// <summary>Reassigns an element to a different content group.</summary>
    public static ContentModelOperationBaseModel ReplaceContentGroup(Reference element, Reference group)
    {
        ArgumentNullException.ThrowIfNull(group);
        return new ContentModelReplacePatchModel { Path = ElementProperty(element, "content_group"), Value = group };
    }

    /// <summary>Adds a content group. With no <paramref name="before"/>/<paramref name="after"/> the group is appended.</summary>
    public static ContentModelOperationBaseModel AddContentGroup(ContentGroupModel group, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(group);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = "/content_groups", Value = group, Before = before, After = after };
    }

    /// <summary>Removes a content group.</summary>
    public static ContentModelOperationBaseModel RemoveContentGroup(Reference group) =>
        new ContentModelRemovePatchModel { Path = ContentGroupPath(group) };

    /// <summary>Moves a content group before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveContentGroupBefore(Reference group, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ContentGroupPath(group), Before = target };
    }

    /// <summary>Moves a content group after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveContentGroupAfter(Reference group, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ContentGroupPath(group), After = target };
    }

    private static string ContentGroupPath(Reference group) => $"/content_groups/{PatchPath.Selector(group)}";
}
