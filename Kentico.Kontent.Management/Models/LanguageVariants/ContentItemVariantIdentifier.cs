using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    /// <summary>
    /// Represents identifier of the content item variant.
    /// </summary>
    public sealed class ContentItemVariantIdentifier
    {
        /// <summary>
        /// Represents identifier of the content item variant.
        /// </summary>
        public Reference ItemIdentifier { get; private set; }

        /// <summary>
        /// Represents identifier of the language.
        /// </summary>
        public NoExternalIdIdentifier LanguageIdentifier { get; private set; }

        /// <summary>
        /// Creates instance of content item variant identifier.
        /// </summary>
        /// <param name="itemIdentifier">The identifier of the content item.</param>
        /// <param name="languageIdentifier">The identifier of the language.</param>
        public ContentItemVariantIdentifier(Reference itemIdentifier, NoExternalIdIdentifier languageIdentifier)
        {
            ItemIdentifier = itemIdentifier;
            LanguageIdentifier = languageIdentifier;
        }
    }
}
