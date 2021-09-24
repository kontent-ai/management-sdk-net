using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    /// <summary>
    /// Represents identifier of the langueage variant.
    /// </summary>
    public sealed class LanguageVariantIdentifier
    {
        /// <summary>
        /// Represents identifier of the language variant.
        /// </summary>
        public Reference ItemIdentifier { get; private set; }

        /// <summary>
        /// Represents identifier of the language.
        /// </summary>
        public Reference LanguageIdentifier { get; private set; }

        /// <summary>
        /// Creates instance of language variant identifier.
        /// </summary>
        /// <param name="itemIdentifier">The identifier of the content item.</param>
        /// <param name="languageIdentifier">The identifier of the language.</param>
        public LanguageVariantIdentifier(Reference itemIdentifier, Reference languageIdentifier)
        {
            ItemIdentifier = itemIdentifier;
            LanguageIdentifier = languageIdentifier;
        }
    }
}
