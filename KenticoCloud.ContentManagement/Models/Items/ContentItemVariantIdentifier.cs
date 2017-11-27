namespace KenticoCloud.ContentManagement
{
    public sealed class ContentItemVariantIdentifier
    {
        // Item identifiers
        public string ItemId { get; set; }
        public string ItemCodename { get; set; }
        public string ItemExternalId { get; set; }

        // Variant identifiers
        public string LanguageId { get; set; }
        public string LanguageCodename { get; set; }

        public ContentItemVariantIdentifier()
        {
        }

        public ContentItemVariantIdentifier(string itemId, string itemCodename, string itemExternalId, string languageId, string languageCodename)
        {
            ItemId = itemId;
            ItemCodename = itemCodename;
            ItemExternalId = itemExternalId;
            LanguageId = languageId;
            LanguageCodename = LanguageCodename;
        }
    }
}
