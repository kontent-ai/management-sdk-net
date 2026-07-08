using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Serialization;

namespace Kontent.Ai.Management.Models.ContentModel.Patch;

/// <summary>
/// Shared intent-revealing factories for content-type and content-type-snippet PATCH operations — every operation
/// except content groups, which are content-type-only. Each method returns a <see cref="ContentModelOperationBaseModel"/>
/// bundling the JSON-Pointer <c>path</c>, the correctly-typed value, and the operation verb so callers never hand-write
/// the wire grammar. The raw <see cref="ContentModelOperationBaseModel.Path"/> string remains available for anything not
/// modeled here. Use the concrete <see cref="Kontent.Ai.Management.Models.Types.Patch.ContentTypePatch"/> /
/// <c>ContentTypeSnippetPatch</c> facades; this base is not instantiable.
/// </summary>
public abstract class ContentModelPatchBase
{
    private static string ElementPath(Reference element) => $"/elements/{PatchPath.Selector(element)}";

    private protected static string ElementProperty(Reference element, string property) => $"{ElementPath(element)}/{property}";

    private protected static void EnsureNotBoth(Reference? before, Reference? after)
    {
        if (before is not null && after is not null)
        {
            throw new ArgumentException("Specify at most one of 'before' or 'after', not both.");
        }
    }

    // ---- Elements ----

    /// <summary>Adds a new element. With no <paramref name="before"/>/<paramref name="after"/> the element is appended.</summary>
    public static ContentModelOperationBaseModel AddElement(ElementMetadataBase element, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(element);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = "/elements", Value = element, Before = before, After = after };
    }

    /// <summary>Removes the element identified by <paramref name="element"/>.</summary>
    public static ContentModelOperationBaseModel RemoveElement(Reference element) =>
        new ContentModelRemovePatchModel { Path = ElementPath(element) };

    /// <summary>Moves <paramref name="element"/> before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveElementBefore(Reference element, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ElementPath(element), Before = target };
    }

    /// <summary>Moves <paramref name="element"/> after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveElementAfter(Reference element, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = ElementPath(element), After = target };
    }

    // ---- Name (replace) ----

    /// <summary>Replaces the display name.</summary>
    public static ContentModelOperationBaseModel ReplaceName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new ContentModelReplacePatchModel { Path = "/name", Value = name };
    }

    // ---- Element properties (replace) ----

    /// <summary>Replaces an element's display name.</summary>
    public static ContentModelOperationBaseModel ReplaceElementName(Reference element, string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new ContentModelReplacePatchModel { Path = ElementProperty(element, "name"), Value = name };
    }

    /// <summary>Replaces an element's guidelines. Pass <c>null</c> to clear them.</summary>
    public static ContentModelOperationBaseModel ReplaceGuidelines(Reference element, string? guidelines) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "guidelines"), Value = guidelines };

    /// <summary>Sets whether an element must be filled in.</summary>
    public static ContentModelOperationBaseModel ReplaceIsRequired(Reference element, bool isRequired) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "is_required"), Value = isRequired };

    /// <summary>Sets whether an element is non-localizable.</summary>
    public static ContentModelOperationBaseModel ReplaceIsNonLocalizable(Reference element, bool isNonLocalizable) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "is_non_localizable"), Value = isNonLocalizable };

    /// <summary>Replaces an element's default value. Pass <c>null</c> to clear it.</summary>
    public static ContentModelOperationBaseModel ReplaceDefault(Reference element, object? value) =>
        new ContentModelReplacePatchModel { Path = ElementProperty(element, "default"), Value = value };

    // ---- Reference collections (per-item add/remove + whole-array replace) ----

    /// <summary>Adds an allowed content type to a linked-items or rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedContentType(Reference element, Reference type) =>
        AddRef(element, "allowed_content_types", type);

    /// <summary>Removes an allowed content type from a linked-items or rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedContentType(Reference element, Reference type) =>
        RemoveRef(element, "allowed_content_types", type);

    /// <summary>Replaces the whole set of allowed content types. An empty set allows all.</summary>
    public static ContentModelOperationBaseModel ReplaceAllowedContentTypes(Reference element, IEnumerable<Reference> types) =>
        ReplaceRefs(element, "allowed_content_types", types);

    /// <summary>Adds an allowed item-link content type to a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedItemLinkType(Reference element, Reference type) =>
        AddRef(element, "allowed_item_link_types", type);

    /// <summary>Removes an allowed item-link content type from a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedItemLinkType(Reference element, Reference type) =>
        RemoveRef(element, "allowed_item_link_types", type);

    /// <summary>Replaces the whole set of allowed item-link content types. An empty set allows all.</summary>
    public static ContentModelOperationBaseModel ReplaceAllowedItemLinkTypes(Reference element, IEnumerable<Reference> types) =>
        ReplaceRefs(element, "allowed_item_link_types", types);

    /// <summary>Adds an allowed element to a custom element.</summary>
    public static ContentModelOperationBaseModel AddAllowedElement(Reference element, Reference allowedElement) =>
        AddRef(element, "allowed_elements", allowedElement);

    /// <summary>Removes an allowed element from a custom element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedElement(Reference element, Reference allowedElement) =>
        RemoveRef(element, "allowed_elements", allowedElement);

    /// <summary>Replaces the whole set of allowed elements on a custom element.</summary>
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

    /// <summary>Adds an option to a multiple-choice element. With no <paramref name="before"/>/<paramref name="after"/> the option is appended.</summary>
    public static ContentModelOperationBaseModel AddOption(Reference element, MultipleChoiceOptionModel option, Reference? before = null, Reference? after = null)
    {
        ArgumentNullException.ThrowIfNull(option);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = ElementProperty(element, "options"), Value = option, Before = before, After = after };
    }

    /// <summary>Removes an option from a multiple-choice element.</summary>
    public static ContentModelOperationBaseModel RemoveOption(Reference element, Reference option) =>
        new ContentModelRemovePatchModel { Path = OptionPath(element, option) };

    /// <summary>Moves an option before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveOptionBefore(Reference element, Reference option, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = OptionPath(element, option), Before = target };
    }

    /// <summary>Moves an option after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveOptionAfter(Reference element, Reference option, Reference target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = OptionPath(element, option), After = target };
    }

    private static string OptionPath(Reference element, Reference option) => $"{ElementProperty(element, "options")}/{PatchPath.Selector(option)}";

    // ---- Rich-text allowed blocks (per-item add/remove only; no whole-array replace) ----

    /// <summary>Allows a block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedBlock(Reference element, RichTextBlockType block) =>
        AddToken(element, "allowed_blocks", block);

    /// <summary>Disallows a block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedBlock(Reference element, RichTextBlockType block) =>
        RemoveToken(element, "allowed_blocks", block);

    /// <summary>Allows a text-block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        AddToken(element, "allowed_text_blocks", block);

    /// <summary>Disallows a text-block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        RemoveToken(element, "allowed_text_blocks", block);

    /// <summary>Allows a text-formatting option in a rich-text element.</summary>
    /// <remarks>When formatting is restricted, the API requires <see cref="RichTextFormattingType.Unstyled"/> to be among the allowed options; include it too, or the operation is rejected.</remarks>
    public static ContentModelOperationBaseModel AddAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        AddToken(element, "allowed_formatting", formatting);

    /// <summary>Disallows a text-formatting option in a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        RemoveToken(element, "allowed_formatting", formatting);

    /// <summary>Allows a block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        AddToken(element, "allowed_table_blocks", block);

    /// <summary>Disallows a block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        RemoveToken(element, "allowed_table_blocks", block);

    /// <summary>Allows a text-block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        AddToken(element, "allowed_table_text_blocks", block);

    /// <summary>Disallows a text-block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        RemoveToken(element, "allowed_table_text_blocks", block);

    /// <summary>Allows a text-formatting option inside tables of a rich-text element.</summary>
    /// <remarks>When table formatting is restricted, the API requires <see cref="RichTextFormattingType.Unstyled"/> to be among the allowed options; include it too, or the operation is rejected.</remarks>
    public static ContentModelOperationBaseModel AddAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        AddToken(element, "allowed_table_formatting", formatting);

    /// <summary>Disallows a text-formatting option inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        RemoveToken(element, "allowed_table_formatting", formatting);

    private static ContentModelOperationBaseModel AddToken<TToken>(Reference element, string collection, TToken token) where TToken : struct, Enum =>
        new ContentModelAddIntoPatchModel { Path = ElementProperty(element, collection), Value = EnumWire.ToValue(token) };

    private static ContentModelOperationBaseModel RemoveToken<TToken>(Reference element, string collection, TToken token) where TToken : struct, Enum =>
        new ContentModelRemovePatchModel { Path = $"{ElementProperty(element, collection)}/{EnumWire.ToValue(token)}" };

    // ---- Escape hatch (raw paths) ----

    /// <summary>Adds <paramref name="value"/> at a raw <paramref name="path"/>, for paths not modeled by a dedicated factory.</summary>
    public static ContentModelOperationBaseModel AddIntoRaw(string path, object value, Reference? before = null, Reference? after = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(value);
        EnsureNotBoth(before, after);
        return new ContentModelAddIntoPatchModel { Path = path, Value = value, Before = before, After = after };
    }

    /// <summary>Replaces the value at a raw <paramref name="path"/>. Pass <c>null</c> to clear.</summary>
    public static ContentModelOperationBaseModel ReplaceRaw(string path, object? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        return new ContentModelReplacePatchModel { Path = path, Value = value };
    }

    /// <summary>Removes the object at a raw <paramref name="path"/>.</summary>
    public static ContentModelOperationBaseModel RemoveRaw(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        return new ContentModelRemovePatchModel { Path = path };
    }

    /// <summary>Moves the object at a raw <paramref name="path"/> before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveRawBefore(string path, Reference target)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = path, Before = target };
    }

    /// <summary>Moves the object at a raw <paramref name="path"/> after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveRawAfter(string path, Reference target)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(target);
        return new ContentModelMovePatchModel { Path = path, After = target };
    }
}
