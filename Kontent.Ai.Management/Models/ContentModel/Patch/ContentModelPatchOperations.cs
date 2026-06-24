using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Serialization;

namespace Kontent.Ai.Management.Models.ContentModel.Patch;

/// <summary>
/// Shared implementation behind the public <c>ContentTypePatch</c> / <c>ContentTypeSnippetPatch</c> factories. Holds
/// the JSON-Pointer path grammar and operation construction in one place; the two facades re-expose the curated
/// subset valid for their target (snippets omit content groups). Content-type and content-type-snippet PATCH share
/// the same operation shapes and element grammar, so there is no per-target operation type.
/// </summary>
internal static class ContentModelPatchOperations
{
    private static string ElementPath(Reference element) => $"/elements/{PatchPath.Selector(element)}";

    private static string ElementProperty(Reference element, string property) => $"{ElementPath(element)}/{property}";

    internal static void EnsureNotBoth(Reference? before, Reference? after)
    {
        if (before is not null && after is not null)
        {
            throw new ArgumentException("Specify at most one of 'before' or 'after', not both.");
        }
    }

    // ---- Elements ----

    public static ContentModelOperationBaseModel AddElement(ElementMetadataBase element, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(element);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = "/elements", Value = element, Before = before, After = after };
    }

    public static ContentModelOperationBaseModel RemoveElement(Reference element) =>
        new ContentModelRemovePatchModel { Path = ElementPath(element) };

    public static ContentModelOperationBaseModel MoveElementBefore(Reference element, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ElementPath(element), Before = target };
    }

    public static ContentModelOperationBaseModel MoveElementAfter(Reference element, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ElementPath(element), After = target };
    }

    // ---- Properties (replace) ----

    public static ContentModelOperationBaseModel ReplaceName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new ContentModelReplacePatchModel { Path = "/name", Value = name };
    }

    public static ContentModelOperationBaseModel ReplaceElementName(Reference element, string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new ContentModelReplacePatchModel { Path = ElementProperty(element, "name"), Value = name };
    }

    public static ContentModelOperationBaseModel ReplaceGuidelines(Reference element, string? guidelines) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "guidelines"), Value = guidelines };

    public static ContentModelOperationBaseModel ReplaceIsRequired(Reference element, bool isRequired) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "is_required"), Value = isRequired };

    public static ContentModelOperationBaseModel ReplaceIsNonLocalizable(Reference element, bool isNonLocalizable) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "is_non_localizable"), Value = isNonLocalizable };

    public static ContentModelOperationBaseModel ReplaceDefault(Reference element, object? value) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "default"), Value = value };

    public static ContentModelOperationBaseModel ReplaceContentGroup(Reference element, Reference group)
    {
        ArgumentNullException.ThrowIfNull(group);
        return new ContentModelReplacePatchModel { Path = ElementProperty(element, "content_group"), Value = group };
    }

    // ---- Reference collections (per-item add/remove + whole-array replace) ----

    public static ContentModelOperationBaseModel AddAllowedContentType(Reference element, Reference type) =>
        AddRef(element, "allowed_content_types", type);

    public static ContentModelOperationBaseModel RemoveAllowedContentType(Reference element, Reference type) =>
        RemoveRef(element, "allowed_content_types", type);

    public static ContentModelOperationBaseModel ReplaceAllowedContentTypes(Reference element, IEnumerable<Reference> types) =>
        ReplaceRefs(element, "allowed_content_types", types);

    public static ContentModelOperationBaseModel AddAllowedItemLinkType(Reference element, Reference type) =>
        AddRef(element, "allowed_item_link_types", type);

    public static ContentModelOperationBaseModel RemoveAllowedItemLinkType(Reference element, Reference type) =>
        RemoveRef(element, "allowed_item_link_types", type);

    public static ContentModelOperationBaseModel ReplaceAllowedItemLinkTypes(Reference element, IEnumerable<Reference> types) =>
        ReplaceRefs(element, "allowed_item_link_types", types);

    public static ContentModelOperationBaseModel AddAllowedElement(Reference element, Reference allowedElement) =>
        AddRef(element, "allowed_elements", allowedElement);

    public static ContentModelOperationBaseModel RemoveAllowedElement(Reference element, Reference allowedElement) =>
        RemoveRef(element, "allowed_elements", allowedElement);

    public static ContentModelOperationBaseModel ReplaceAllowedElements(Reference element, IEnumerable<Reference> allowedElements) =>
        ReplaceRefs(element, "allowed_elements", allowedElements);

    private static ContentModelOperationBaseModel AddRef(Reference element, string collection, Reference value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new ContentModelAddIntoPatchModel { Path = ElementProperty(element, collection), Value = value };
    }

    private static ContentModelOperationBaseModel RemoveRef(Reference element, string collection, Reference value) =>
        new ContentModelRemovePatchModel { Path = $"{ElementProperty(element, collection)}/{PatchPath.Selector(value)}" };

    private static ContentModelOperationBaseModel ReplaceRefs(Reference element, string collection, IEnumerable<Reference> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return new ContentModelReplacePatchModel { Path = ElementProperty(element, collection), Value = values };
    }

    // ---- Multiple-choice options ----

    public static ContentModelOperationBaseModel AddOption(Reference element, MultipleChoiceOptionModel option, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(option);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = ElementProperty(element, "options"), Value = option, Before = before, After = after };
    }

    public static ContentModelOperationBaseModel RemoveOption(Reference element, Reference option) =>
        new ContentModelRemovePatchModel { Path = OptionPath(element, option) };

    public static ContentModelOperationBaseModel MoveOptionBefore(Reference element, Reference option, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = OptionPath(element, option), Before = target };
    }

    public static ContentModelOperationBaseModel MoveOptionAfter(Reference element, Reference option, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = OptionPath(element, option), After = target };
    }

    private static string OptionPath(Reference element, Reference option) => $"{ElementProperty(element, "options")}/{PatchPath.Selector(option)}";

    // ---- Content groups (content types only) ----

    public static ContentModelOperationBaseModel AddContentGroup(ContentGroupModel group, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(group);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = "/content_groups", Value = group, Before = before, After = after };
    }

    public static ContentModelOperationBaseModel RemoveContentGroup(Reference group) =>
        new ContentModelRemovePatchModel { Path = ContentGroupPath(group) };

    public static ContentModelOperationBaseModel MoveContentGroupBefore(Reference group, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ContentGroupPath(group), Before = target };
    }

    public static ContentModelOperationBaseModel MoveContentGroupAfter(Reference group, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ContentGroupPath(group), After = target };
    }

    private static string ContentGroupPath(Reference group) => $"/content_groups/{PatchPath.Selector(group)}";

    // ---- Rich-text allowed blocks (per-item add/remove only; no whole-array replace) ----

    public static ContentModelOperationBaseModel AddAllowedBlock(Reference element, RichTextBlockType block) =>
        AddToken(element, "allowed_blocks", block);

    public static ContentModelOperationBaseModel RemoveAllowedBlock(Reference element, RichTextBlockType block) =>
        RemoveToken(element, "allowed_blocks", block);

    public static ContentModelOperationBaseModel AddAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        AddToken(element, "allowed_text_blocks", block);

    public static ContentModelOperationBaseModel RemoveAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        RemoveToken(element, "allowed_text_blocks", block);

    public static ContentModelOperationBaseModel AddAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        AddToken(element, "allowed_formatting", formatting);

    public static ContentModelOperationBaseModel RemoveAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        RemoveToken(element, "allowed_formatting", formatting);

    public static ContentModelOperationBaseModel AddAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        AddToken(element, "allowed_table_blocks", block);

    public static ContentModelOperationBaseModel RemoveAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        RemoveToken(element, "allowed_table_blocks", block);

    public static ContentModelOperationBaseModel AddAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        AddToken(element, "allowed_table_text_blocks", block);

    public static ContentModelOperationBaseModel RemoveAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        RemoveToken(element, "allowed_table_text_blocks", block);

    public static ContentModelOperationBaseModel AddAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        AddToken(element, "allowed_table_formatting", formatting);

    public static ContentModelOperationBaseModel RemoveAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        RemoveToken(element, "allowed_table_formatting", formatting);

    private static ContentModelOperationBaseModel AddToken<TToken>(Reference element, string collection, TToken token) where TToken : struct, Enum =>
        new ContentModelAddIntoPatchModel { Path = ElementProperty(element, collection), Value = EnumWire.ToValue(token) };

    private static ContentModelOperationBaseModel RemoveToken<TToken>(Reference element, string collection, TToken token) where TToken : struct, Enum =>
        new ContentModelRemovePatchModel { Path = $"{ElementProperty(element, collection)}/{EnumWire.ToValue(token)}" };

    // ---- Escape hatch (raw paths) ----

    public static ContentModelOperationBaseModel AddIntoRaw(string path, object value, Reference? before = null, Reference? after = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(value);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = path, Value = value, Before = before, After = after };
    }

    public static ContentModelOperationBaseModel ReplaceRaw(string path, object? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        return new ContentModelReplacePatchModel { Path = path, Value = value };
    }

    public static ContentModelOperationBaseModel RemoveRaw(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        return new ContentModelRemovePatchModel { Path = path };
    }

    public static ContentModelOperationBaseModel MoveRawBefore(string path, Reference target)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = path, Before = target };
    }

    public static ContentModelOperationBaseModel MoveRawAfter(string path, Reference target)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = path, After = target };
    }
}
