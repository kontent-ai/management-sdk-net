using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Identifies a language variant by its content item and language.
/// </summary>
public sealed record LanguageVariantIdentifier
{
    /// <summary>
    /// Reference to the content item.
    /// </summary>
    public required Reference ItemIdentifier { get; init; }

    /// <summary>
    /// Reference to the language.
    /// </summary>
    public required Reference LanguageIdentifier { get; init; }

    /// <summary>
    /// Creates an identifier from the content item and language references.
    /// </summary>
    [SetsRequiredMembers]
    public LanguageVariantIdentifier(Reference itemIdentifier, Reference languageIdentifier)
    {
        ItemIdentifier = itemIdentifier;
        LanguageIdentifier = languageIdentifier;
    }

    /// <summary>Creates an identifier from the content item and language codenames.</summary>
    public static LanguageVariantIdentifier ByCodenames(string itemCodename, string languageCodename)
        => new(Reference.ByCodename(itemCodename), Reference.ByCodename(languageCodename));

    /// <summary>Creates an identifier from the content item and language IDs.</summary>
    public static LanguageVariantIdentifier ByIds(Guid itemId, Guid languageId)
        => new(Reference.ById(itemId), Reference.ById(languageId));

    /// <summary>Creates an identifier from the content item and language external IDs.</summary>
    public static LanguageVariantIdentifier ByExternalIds(string itemExternalId, string languageExternalId)
        => new(Reference.ByExternalId(itemExternalId), Reference.ByExternalId(languageExternalId));
}
