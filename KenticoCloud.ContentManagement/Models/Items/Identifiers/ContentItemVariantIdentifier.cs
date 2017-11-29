namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantIdentifier
    {
        public ContentItemIdentifier ItemIdentifier { get; private set; }
        public LanguageIdentifier LanguageIdentifier { get; private set; }

        public ContentItemVariantIdentifier(ContentItemIdentifier itemIdentifier, LanguageIdentifier languageIdentifier)
        {
            ItemIdentifier = itemIdentifier;
            LanguageIdentifier = languageIdentifier;
        }
    }
}
