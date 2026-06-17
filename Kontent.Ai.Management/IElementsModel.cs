namespace Kontent.Ai.Management;

/// <summary>
/// Marker for a generated content-type record — the strongly-typed model of a content type's <em>elements</em>
/// (the variant/component values), carrying no item or variant metadata. Records emitted by
/// <c>model-generator-net</c>'s management mode implement this so the SDK validator, STJ envelope converter, and
/// strongly-typed API methods can recognize them. Variant-level metadata lives on <c>LanguageVariantModel&lt;T&gt;</c>.
/// </summary>
public interface IElementsModel
{
}
