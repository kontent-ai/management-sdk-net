using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents the identifier of the language variant.
/// </summary>
public sealed class LanguageVariantIdentifier
{
    /// <summary>
    /// Represents the identifier of the language variant.
    /// </summary>
    public Reference ItemIdentifier { get; private set; }

    /// <summary>
    /// Represents the identifier of the language.
    /// </summary>
    public Reference LanguageIdentifier { get; private set; }

    /// <summary>
    /// Creates an instance of language variant identifier.
    /// </summary>
    /// <param name="itemIdentifier">The identifier of the content item.</param>
    /// <param name="languageIdentifier">The identifier of the language.</param>
    public LanguageVariantIdentifier(Reference itemIdentifier, Reference languageIdentifier)
    {
        ItemIdentifier = itemIdentifier;
        LanguageIdentifier = languageIdentifier;
    }
}
