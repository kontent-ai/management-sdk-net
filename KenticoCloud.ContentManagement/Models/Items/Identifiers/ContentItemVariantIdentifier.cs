namespace KenticoCloud.ContentManagement
{
    public sealed class ContentItemVariantIdentifier
    {
        public ContentItemIdentifier ItemIdentifier { get; private set; }
        public ContentVariantIdentifier VariantIdentifier { get; private set; }

        public ContentItemVariantIdentifier(ContentItemIdentifier itemIdentifier, ContentVariantIdentifier variantIdentifier)
        {
            ItemIdentifier = itemIdentifier;
            VariantIdentifier = variantIdentifier;
        }
    }
}
