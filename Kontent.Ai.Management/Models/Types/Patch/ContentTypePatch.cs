using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Serialization;

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Intent-revealing factories for content-type PATCH operations. Each method returns a
/// <see cref="ContentTypeOperationBaseModel"/> ready to pass to <c>ModifyContentTypeAsync</c>, bundling
/// the JSON-Pointer <c>path</c>, the correctly-typed value, and the operation verb so callers never hand-write
/// the wire grammar. The raw <see cref="ContentTypeOperationBaseModel.Path"/> string remains available for
/// anything not modeled here.
/// </summary>
public static class ContentTypePatch
{
    private static string ElementPath(Reference element) => $"/elements/{PatchPath.Selector(element)}";

    private static string ElementProperty(Reference element, string property) => $"{ElementPath(element)}/{property}";

    private static void EnsureNotBoth(Reference? before, Reference? after)
    {
        if (before is not null && after is not null)
        {
            throw new ArgumentException("Specify at most one of 'before' or 'after', not both.");
        }
    }

    // ---- Elements ----

    /// <summary>Adds a new element. With no <paramref name="before"/>/<paramref name="after"/> the element is appended.</summary>
    public static ContentTypeOperationBaseModel AddElement(ElementMetadataBase element, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(element);
        EnsureNotBoth(before, after);
        return new ContentTypeAddIntoPatchModel { Path = "/elements", Value = element, Before = before, After = after };
    }

    /// <summary>Removes the element identified by <paramref name="element"/>.</summary>
    public static ContentTypeOperationBaseModel RemoveElement(Reference element) =>
        new ContentTypeRemovePatchModel { Path = ElementPath(element) };

    /// <summary>Moves <paramref name="element"/> before <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveElementBefore(Reference element, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = ElementPath(element), Before = target };
    }

    /// <summary>Moves <paramref name="element"/> after <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveElementAfter(Reference element, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = ElementPath(element), After = target };
    }

    // ---- Element properties (replace) ----

    /// <summary>Replaces the content type's display name.</summary>
    public static ContentTypeOperationBaseModel ReplaceName(string name) =>
        new ContentTypeReplacePatchModel { Path = "/name", Value = name };

    /// <summary>Replaces an element's display name.</summary>
    public static ContentTypeOperationBaseModel ReplaceElementName(Reference element, string name) =>
        new ContentTypeReplacePatchModel { Path = ElementProperty(element, "name"), Value = name };

    /// <summary>Replaces an element's guidelines. Pass <c>null</c> to clear them.</summary>
    public static ContentTypeOperationBaseModel ReplaceGuidelines(Reference element, string? guidelines) =>
        new ContentTypeReplacePatchModel { Path = ElementProperty(element, "guidelines"), Value = guidelines };

    /// <summary>Sets whether an element must be filled in.</summary>
    public static ContentTypeOperationBaseModel ReplaceIsRequired(Reference element, bool isRequired) =>
        new ContentTypeReplacePatchModel { Path = ElementProperty(element, "is_required"), Value = isRequired };

    /// <summary>Sets whether an element is non-localizable.</summary>
    public static ContentTypeOperationBaseModel ReplaceIsNonLocalizable(Reference element, bool isNonLocalizable) =>
        new ContentTypeReplacePatchModel { Path = ElementProperty(element, "is_non_localizable"), Value = isNonLocalizable };

    /// <summary>Replaces an element's default value. Pass <c>null</c> to clear it.</summary>
    public static ContentTypeOperationBaseModel ReplaceDefault(Reference element, object? value) =>
        new ContentTypeReplacePatchModel { Path = ElementProperty(element, "default"), Value = value };

    /// <summary>Reassigns an element to a different content group.</summary>
    public static ContentTypeOperationBaseModel ReplaceContentGroup(Reference element, Reference group)
    {
        ArgumentNullException.ThrowIfNull(group);
        return new ContentTypeReplacePatchModel { Path = ElementProperty(element, "content_group"), Value = group };
    }

    // ---- Reference collections (per-item add/remove + whole-array replace) ----

    /// <summary>Adds an allowed content type to a linked-items or rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedContentType(Reference element, Reference type) =>
        AddRef(element, "allowed_content_types", type);

    /// <summary>Removes an allowed content type from a linked-items or rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedContentType(Reference element, Reference type) =>
        RemoveRef(element, "allowed_content_types", type);

    /// <summary>Replaces the whole set of allowed content types. An empty set allows all.</summary>
    public static ContentTypeOperationBaseModel ReplaceAllowedContentTypes(Reference element, IEnumerable<Reference> types) =>
        ReplaceRefs(element, "allowed_content_types", types);

    /// <summary>Adds an allowed item-link content type to a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedItemLinkType(Reference element, Reference type) =>
        AddRef(element, "allowed_item_link_types", type);

    /// <summary>Removes an allowed item-link content type from a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedItemLinkType(Reference element, Reference type) =>
        RemoveRef(element, "allowed_item_link_types", type);

    /// <summary>Replaces the whole set of allowed item-link content types. An empty set allows all.</summary>
    public static ContentTypeOperationBaseModel ReplaceAllowedItemLinkTypes(Reference element, IEnumerable<Reference> types) =>
        ReplaceRefs(element, "allowed_item_link_types", types);

    /// <summary>Adds an allowed element to a custom element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedElement(Reference element, Reference allowedElement) =>
        AddRef(element, "allowed_elements", allowedElement);

    /// <summary>Removes an allowed element from a custom element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedElement(Reference element, Reference allowedElement) =>
        RemoveRef(element, "allowed_elements", allowedElement);

    /// <summary>Replaces the whole set of allowed elements on a custom element.</summary>
    public static ContentTypeOperationBaseModel ReplaceAllowedElements(Reference element, IEnumerable<Reference> allowedElements) =>
        ReplaceRefs(element, "allowed_elements", allowedElements);

    private static ContentTypeOperationBaseModel AddRef(Reference element, string collection, Reference value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new ContentTypeAddIntoPatchModel { Path = ElementProperty(element, collection), Value = value };
    }

    private static ContentTypeOperationBaseModel RemoveRef(Reference element, string collection, Reference value) =>
        new ContentTypeRemovePatchModel { Path = $"{ElementProperty(element, collection)}/{PatchPath.Selector(value)}" };

    private static ContentTypeOperationBaseModel ReplaceRefs(Reference element, string collection, IEnumerable<Reference> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return new ContentTypeReplacePatchModel { Path = ElementProperty(element, collection), Value = values };
    }

    // ---- Multiple-choice options ----

    /// <summary>Adds an option to a multiple-choice element. With no <paramref name="before"/>/<paramref name="after"/> the option is appended.</summary>
    public static ContentTypeOperationBaseModel AddOption(Reference element, MultipleChoiceOptionModel option, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(option);
        EnsureNotBoth(before, after);
        return new ContentTypeAddIntoPatchModel { Path = ElementProperty(element, "options"), Value = option, Before = before, After = after };
    }

    /// <summary>Removes an option from a multiple-choice element.</summary>
    public static ContentTypeOperationBaseModel RemoveOption(Reference element, Reference option) =>
        new ContentTypeRemovePatchModel { Path = OptionPath(element, option) };

    /// <summary>Moves an option before <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveOptionBefore(Reference element, Reference option, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = OptionPath(element, option), Before = target };
    }

    /// <summary>Moves an option after <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveOptionAfter(Reference element, Reference option, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = OptionPath(element, option), After = target };
    }

    private static string OptionPath(Reference element, Reference option) => $"{ElementProperty(element, "options")}/{PatchPath.Selector(option)}";

    // ---- Content groups ----

    /// <summary>Adds a content group. With no <paramref name="before"/>/<paramref name="after"/> the group is appended.</summary>
    public static ContentTypeOperationBaseModel AddContentGroup(ContentGroupModel group, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(group);
        EnsureNotBoth(before, after);
        return new ContentTypeAddIntoPatchModel { Path = "/content_groups", Value = group, Before = before, After = after };
    }

    /// <summary>Removes a content group.</summary>
    public static ContentTypeOperationBaseModel RemoveContentGroup(Reference group) =>
        new ContentTypeRemovePatchModel { Path = ContentGroupPath(group) };

    /// <summary>Moves a content group before <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveContentGroupBefore(Reference group, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = ContentGroupPath(group), Before = target };
    }

    /// <summary>Moves a content group after <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveContentGroupAfter(Reference group, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = ContentGroupPath(group), After = target };
    }

    private static string ContentGroupPath(Reference group) => $"/content_groups/{PatchPath.Selector(group)}";

    // ---- Rich-text allowed blocks (per-item add/remove only; no whole-array replace) ----

    /// <summary>Allows a block type in a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedBlock(Reference element, RichTextBlockType block) =>
        AddToken(element, "allowed_blocks", block);

    /// <summary>Disallows a block type in a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedBlock(Reference element, RichTextBlockType block) =>
        RemoveToken(element, "allowed_blocks", block);

    /// <summary>Allows a text-block type in a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        AddToken(element, "allowed_text_blocks", block);

    /// <summary>Disallows a text-block type in a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        RemoveToken(element, "allowed_text_blocks", block);

    /// <summary>Allows a text-formatting option in a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        AddToken(element, "allowed_formatting", formatting);

    /// <summary>Disallows a text-formatting option in a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        RemoveToken(element, "allowed_formatting", formatting);

    /// <summary>Allows a block type inside tables of a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        AddToken(element, "allowed_table_blocks", block);

    /// <summary>Disallows a block type inside tables of a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        RemoveToken(element, "allowed_table_blocks", block);

    /// <summary>Allows a text-block type inside tables of a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        AddToken(element, "allowed_table_text_blocks", block);

    /// <summary>Disallows a text-block type inside tables of a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        RemoveToken(element, "allowed_table_text_blocks", block);

    /// <summary>Allows a text-formatting option inside tables of a rich-text element.</summary>
    public static ContentTypeOperationBaseModel AddAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        AddToken(element, "allowed_table_formatting", formatting);

    /// <summary>Disallows a text-formatting option inside tables of a rich-text element.</summary>
    public static ContentTypeOperationBaseModel RemoveAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        RemoveToken(element, "allowed_table_formatting", formatting);

    private static ContentTypeOperationBaseModel AddToken<TToken>(Reference element, string collection, TToken token) where TToken : struct, Enum =>
        new ContentTypeAddIntoPatchModel { Path = ElementProperty(element, collection), Value = EnumWire.ToValue(token) };

    private static ContentTypeOperationBaseModel RemoveToken<TToken>(Reference element, string collection, TToken token) where TToken : struct, Enum =>
        new ContentTypeRemovePatchModel { Path = $"{ElementProperty(element, collection)}/{EnumWire.ToValue(token)}" };

    // ---- Escape hatch (raw paths) ----

    /// <summary>Adds <paramref name="value"/> at a raw <paramref name="path"/>, for paths not modeled by a dedicated factory.</summary>
    public static ContentTypeOperationBaseModel AddIntoRaw(string path, object value, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(value);
        EnsureNotBoth(before, after);
        return new ContentTypeAddIntoPatchModel { Path = path, Value = value, Before = before, After = after };
    }

    /// <summary>Replaces the value at a raw <paramref name="path"/>. Pass <c>null</c> to clear.</summary>
    public static ContentTypeOperationBaseModel ReplaceRaw(string path, object? value) =>
        new ContentTypeReplacePatchModel { Path = path, Value = value };

    /// <summary>Removes the object at a raw <paramref name="path"/>.</summary>
    public static ContentTypeOperationBaseModel RemoveRaw(string path) =>
        new ContentTypeRemovePatchModel { Path = path };

    /// <summary>Moves the object at a raw <paramref name="path"/> before <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveRawBefore(string path, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = path, Before = target };
    }

    /// <summary>Moves the object at a raw <paramref name="path"/> after <paramref name="target"/>.</summary>
    public static ContentTypeOperationBaseModel MoveRawAfter(string path, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentTypeMovePatchModel { Path = path, After = target };
    }
}
