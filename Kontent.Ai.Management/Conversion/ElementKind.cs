namespace Kontent.Ai.Management.Conversion;

/// <summary>
/// Discriminator for how the envelope converter (de)serializes a content-type record property's value.
/// Derived from the property's CLR type — see <see cref="ContentItemTypeDescriptor"/>.
/// </summary>
internal enum ElementKind
{
    /// <summary><see cref="string"/>; text elements.</summary>
    Text,

    /// <summary><see cref="decimal"/>; number elements.</summary>
    Number,

    /// <summary><see cref="Models.Content.DateTimeValue"/>; date_time elements (instant + display_timezone).</summary>
    DateTime,

    /// <summary><see cref="Models.Content.UrlSlugValue"/>; url_slug elements (slug + mode).</summary>
    UrlSlug,

    /// <summary><see cref="Models.Content.CustomValue"/>; custom elements (value + searchable_value).</summary>
    Custom,

    /// <summary><see cref="System.Collections.Generic.IReadOnlyList{T}"/> of an <c>[KontentEnumValue]</c>-annotated enum; multiple_choice elements.</summary>
    MultipleChoice,

    /// <summary><see cref="System.Collections.Generic.IReadOnlyList{T}"/> of <see cref="Models.Content.AssetReference"/>; asset elements.</summary>
    Asset,

    /// <summary>
    /// <see cref="System.Collections.Generic.IReadOnlyList{T}"/> of <see cref="Models.Shared.Reference"/>; covers
    /// taxonomy, modular_content, subpages, and item-link snippet elements — all share the same identifier-array wire shape.
    /// </summary>
    Reference,

    /// <summary><see cref="Models.Content.RichTextElement"/>; rich_text elements (with optional embedded components).</summary>
    RichText,
}
