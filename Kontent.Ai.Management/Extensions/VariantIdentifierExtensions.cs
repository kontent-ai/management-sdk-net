using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Converts fetched variant-shaped models into the identifiers other calls expect, so a variant found by a listing
/// or filter can be fed straight back into publish/upsert/workflow calls without reassembling its item and language.
/// </summary>
public static class VariantIdentifierExtensions
{
    /// <summary>Creates a <see cref="LanguageVariantIdentifier"/> from this variant's item and language.</summary>
    /// <remarks>The references carry whatever the response carried — typically ids, which are environment-specific.</remarks>
    public static LanguageVariantIdentifier ToIdentifier(this LanguageVariantMetadata variant)
    {
        ArgumentNullException.ThrowIfNull(variant);
        return new LanguageVariantIdentifier(variant.Item, variant.Language);
    }

    /// <summary>Creates a <see cref="LanguageVariantIdentifier"/> from this filter result's item and language.</summary>
    /// <remarks>The references carry whatever the response carried — typically ids, which are environment-specific.</remarks>
    public static LanguageVariantIdentifier ToIdentifier(this ItemWithVariantFilterResultModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return new LanguageVariantIdentifier(model.Item, model.Language);
    }

    /// <summary>
    /// Creates a <see cref="VariantIdentifierModel"/> from this filter result's item and language — for feeding
    /// filter results into an items-with-variants bulk get.
    /// </summary>
    public static VariantIdentifierModel ToVariantIdentifier(this ItemWithVariantFilterResultModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return new VariantIdentifierModel { Item = model.Item, Language = model.Language };
    }
}
