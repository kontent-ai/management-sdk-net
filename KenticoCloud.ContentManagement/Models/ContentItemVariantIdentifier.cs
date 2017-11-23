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
    }
}
