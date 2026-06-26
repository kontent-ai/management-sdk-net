using Kontent.Ai.Management.Models.Types.Elements;

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
    // ---- Elements ----

    /// <summary>Adds a new element. With no <paramref name="before"/>/<paramref name="after"/> the element is appended.</summary>
    public static ContentModelOperationBaseModel AddElement(ElementMetadataBase element, Reference? before = null, Reference? after = null) =>
        ContentModelPatchOperations.AddElement(element, before, after);

    /// <summary>Removes the element identified by <paramref name="element"/>.</summary>
    public static ContentModelOperationBaseModel RemoveElement(Reference element) =>
        ContentModelPatchOperations.RemoveElement(element);

    /// <summary>Moves <paramref name="element"/> before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveElementBefore(Reference element, Reference target) =>
        ContentModelPatchOperations.MoveElementBefore(element, target);

    /// <summary>Moves <paramref name="element"/> after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveElementAfter(Reference element, Reference target) =>
        ContentModelPatchOperations.MoveElementAfter(element, target);

    // ---- Element properties (replace) ----

    /// <summary>Replaces the display name.</summary>
    public static ContentModelOperationBaseModel ReplaceName(string name) =>
        ContentModelPatchOperations.ReplaceName(name);

    /// <summary>Replaces an element's display name.</summary>
    public static ContentModelOperationBaseModel ReplaceElementName(Reference element, string name) =>
        ContentModelPatchOperations.ReplaceElementName(element, name);

    /// <summary>Replaces an element's guidelines. Pass <c>null</c> to clear them.</summary>
    public static ContentModelOperationBaseModel ReplaceGuidelines(Reference element, string? guidelines) =>
        ContentModelPatchOperations.ReplaceGuidelines(element, guidelines);

    /// <summary>Sets whether an element must be filled in.</summary>
    public static ContentModelOperationBaseModel ReplaceIsRequired(Reference element, bool isRequired) =>
        ContentModelPatchOperations.ReplaceIsRequired(element, isRequired);

    /// <summary>Sets whether an element is non-localizable.</summary>
    public static ContentModelOperationBaseModel ReplaceIsNonLocalizable(Reference element, bool isNonLocalizable) =>
        ContentModelPatchOperations.ReplaceIsNonLocalizable(element, isNonLocalizable);

    /// <summary>Replaces an element's default value. Pass <c>null</c> to clear it.</summary>
    public static ContentModelOperationBaseModel ReplaceDefault(Reference element, object? value) =>
        ContentModelPatchOperations.ReplaceDefault(element, value);

    // ---- Reference collections (per-item add/remove + whole-array replace) ----

    /// <summary>Adds an allowed content type to a linked-items or rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedContentType(Reference element, Reference type) =>
        ContentModelPatchOperations.AddAllowedContentType(element, type);

    /// <summary>Removes an allowed content type from a linked-items or rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedContentType(Reference element, Reference type) =>
        ContentModelPatchOperations.RemoveAllowedContentType(element, type);

    /// <summary>Replaces the whole set of allowed content types. An empty set allows all.</summary>
    public static ContentModelOperationBaseModel ReplaceAllowedContentTypes(Reference element, IEnumerable<Reference> types) =>
        ContentModelPatchOperations.ReplaceAllowedContentTypes(element, types);

    /// <summary>Adds an allowed item-link content type to a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedItemLinkType(Reference element, Reference type) =>
        ContentModelPatchOperations.AddAllowedItemLinkType(element, type);

    /// <summary>Removes an allowed item-link content type from a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedItemLinkType(Reference element, Reference type) =>
        ContentModelPatchOperations.RemoveAllowedItemLinkType(element, type);

    /// <summary>Replaces the whole set of allowed item-link content types. An empty set allows all.</summary>
    public static ContentModelOperationBaseModel ReplaceAllowedItemLinkTypes(Reference element, IEnumerable<Reference> types) =>
        ContentModelPatchOperations.ReplaceAllowedItemLinkTypes(element, types);

    /// <summary>Adds an allowed element to a custom element.</summary>
    public static ContentModelOperationBaseModel AddAllowedElement(Reference element, Reference allowedElement) =>
        ContentModelPatchOperations.AddAllowedElement(element, allowedElement);

    /// <summary>Removes an allowed element from a custom element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedElement(Reference element, Reference allowedElement) =>
        ContentModelPatchOperations.RemoveAllowedElement(element, allowedElement);

    /// <summary>Replaces the whole set of allowed elements on a custom element.</summary>
    public static ContentModelOperationBaseModel ReplaceAllowedElements(Reference element, IEnumerable<Reference> allowedElements) =>
        ContentModelPatchOperations.ReplaceAllowedElements(element, allowedElements);

    // ---- Multiple-choice options ----

    /// <summary>Adds an option to a multiple-choice element. With no <paramref name="before"/>/<paramref name="after"/> the option is appended.</summary>
    public static ContentModelOperationBaseModel AddOption(Reference element, MultipleChoiceOptionModel option, Reference? before = null, Reference? after = null) =>
        ContentModelPatchOperations.AddOption(element, option, before, after);

    /// <summary>Removes an option from a multiple-choice element.</summary>
    public static ContentModelOperationBaseModel RemoveOption(Reference element, Reference option) =>
        ContentModelPatchOperations.RemoveOption(element, option);

    /// <summary>Moves an option before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveOptionBefore(Reference element, Reference option, Reference target) =>
        ContentModelPatchOperations.MoveOptionBefore(element, option, target);

    /// <summary>Moves an option after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveOptionAfter(Reference element, Reference option, Reference target) =>
        ContentModelPatchOperations.MoveOptionAfter(element, option, target);

    // ---- Rich-text allowed blocks (per-item add/remove only; no whole-array replace) ----

    /// <summary>Allows a block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedBlock(Reference element, RichTextBlockType block) =>
        ContentModelPatchOperations.AddAllowedBlock(element, block);

    /// <summary>Disallows a block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedBlock(Reference element, RichTextBlockType block) =>
        ContentModelPatchOperations.RemoveAllowedBlock(element, block);

    /// <summary>Allows a text-block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        ContentModelPatchOperations.AddAllowedTextBlock(element, block);

    /// <summary>Disallows a text-block type in a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTextBlock(Reference element, RichTextTextBlockType block) =>
        ContentModelPatchOperations.RemoveAllowedTextBlock(element, block);

    /// <summary>Allows a text-formatting option in a rich-text element.</summary>
    /// <remarks>When formatting is restricted, the API requires <see cref="RichTextFormattingType.Unstyled"/> to be among the allowed options; include it too, or the operation is rejected.</remarks>
    public static ContentModelOperationBaseModel AddAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        ContentModelPatchOperations.AddAllowedFormatting(element, formatting);

    /// <summary>Disallows a text-formatting option in a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedFormatting(Reference element, RichTextFormattingType formatting) =>
        ContentModelPatchOperations.RemoveAllowedFormatting(element, formatting);

    /// <summary>Allows a block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        ContentModelPatchOperations.AddAllowedTableBlock(element, block);

    /// <summary>Disallows a block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTableBlock(Reference element, RichTextTableBlockType block) =>
        ContentModelPatchOperations.RemoveAllowedTableBlock(element, block);

    /// <summary>Allows a text-block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel AddAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        ContentModelPatchOperations.AddAllowedTableTextBlock(element, block);

    /// <summary>Disallows a text-block type inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTableTextBlock(Reference element, RichTextTextBlockType block) =>
        ContentModelPatchOperations.RemoveAllowedTableTextBlock(element, block);

    /// <summary>Allows a text-formatting option inside tables of a rich-text element.</summary>
    /// <remarks>When table formatting is restricted, the API requires <see cref="RichTextFormattingType.Unstyled"/> to be among the allowed options; include it too, or the operation is rejected.</remarks>
    public static ContentModelOperationBaseModel AddAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        ContentModelPatchOperations.AddAllowedTableFormatting(element, formatting);

    /// <summary>Disallows a text-formatting option inside tables of a rich-text element.</summary>
    public static ContentModelOperationBaseModel RemoveAllowedTableFormatting(Reference element, RichTextFormattingType formatting) =>
        ContentModelPatchOperations.RemoveAllowedTableFormatting(element, formatting);

    // ---- Escape hatch (raw paths) ----

    /// <summary>Adds <paramref name="value"/> at a raw <paramref name="path"/>, for paths not modeled by a dedicated factory.</summary>
    public static ContentModelOperationBaseModel AddIntoRaw(string path, object value, Reference? before = null, Reference? after = null) =>
        ContentModelPatchOperations.AddIntoRaw(path, value, before, after);

    /// <summary>Replaces the value at a raw <paramref name="path"/>. Pass <c>null</c> to clear.</summary>
    public static ContentModelOperationBaseModel ReplaceRaw(string path, object? value) =>
        ContentModelPatchOperations.ReplaceRaw(path, value);

    /// <summary>Removes the object at a raw <paramref name="path"/>.</summary>
    public static ContentModelOperationBaseModel RemoveRaw(string path) =>
        ContentModelPatchOperations.RemoveRaw(path);

    /// <summary>Moves the object at a raw <paramref name="path"/> before <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveRawBefore(string path, Reference target) =>
        ContentModelPatchOperations.MoveRawBefore(path, target);

    /// <summary>Moves the object at a raw <paramref name="path"/> after <paramref name="target"/>.</summary>
    public static ContentModelOperationBaseModel MoveRawAfter(string path, Reference target) =>
        ContentModelPatchOperations.MoveRawAfter(path, target);
}
