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

    [SetsRequiredMembers]
    public LanguageVariantIdentifier(Reference itemIdentifier, Reference languageIdentifier)
    {
        ItemIdentifier = itemIdentifier;
        LanguageIdentifier = languageIdentifier;
    }
}
