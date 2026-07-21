namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// A language variant with strongly-typed elements — the typed counterpart of <see cref="LanguageVariantModel"/>.
/// Returned by the generic <c>GetLanguageVariantAsync&lt;T&gt;</c> / <c>UpsertLanguageVariantAsync&lt;T&gt;</c>: the variant's
/// element values are projected onto <typeparamref name="TModel"/>, while the metadata is preserved rather than discarded.
/// A client-side projection — not deserialized from the wire.
/// </summary>
/// <typeparam name="TModel">The generated content-type record modeling this variant's elements.</typeparam>
public sealed record LanguageVariantModel<TModel> : LanguageVariantMetadata where TModel : IElementsModel
{
    /// <summary>The strongly-typed element values.</summary>
    public required TModel Elements { get; init; }
}
