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
    public static ContentModelOperationBaseModel ReplaceContentGroup(Reference element, Reference group) =>
        ContentModelPatchOperations.ReplaceContentGroup(element, group);

    /// <summary>Adds a content group. With no <paramref name="before"/>/<paramref name="after"/> the group is appended.</summary>
    public static ContentModelOperationBaseModel AddContentGroup(ContentGroupModel group, Reference? before = null, Reference? after = null) =>
        ContentModelPatchOperations.AddContentGroup(group, before, after);

    /// <summary>Removes a content group.</summary>
    public static ContentModelOperationBaseModel RemoveContentGroup(Reference group) =>
        ContentModelPatchOperations.RemoveContentGroup(group);

    /// <summary>Moves a content group before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveContentGroupBefore(Reference group, Reference target) =>
        ContentModelPatchOperations.MoveContentGroupBefore(group, target);

    /// <summary>Moves a content group after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveContentGroupAfter(Reference group, Reference target) =>
        ContentModelPatchOperations.MoveContentGroupAfter(group, target);
}
