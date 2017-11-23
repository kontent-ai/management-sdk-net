namespace KenticoCloud.ContentManagement
{
    public sealed class ContentItemIdentifier
    {
        public string ItemId { get; set; }
        public string ItemCodename { get; set; }
        public string ItemExternalId { get; set; }

        public ContentItemIdentifier(string itemId, string itemCodename, string itemExternalId)
        {
            ItemId = itemId;
            ItemCodename = itemCodename;
            ItemExternalId = itemExternalId;

        }
    }
}
