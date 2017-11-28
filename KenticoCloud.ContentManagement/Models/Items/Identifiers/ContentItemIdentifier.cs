using System;

namespace KenticoCloud.ContentManagement
{
    public class ContentItemIdentifier
    {
        public Guid ItemId { get; private set; }
        public string ItemCodename { get; private set; }
        public string ItemExternalId { get; private set; }

        public static ContentItemIdentifier ById(Guid itemId) {
            return new ContentItemIdentifier() { ItemId = itemId };
        }

        public static ContentItemIdentifier ByCodename(string codename)
        {
            return new ContentItemIdentifier() { ItemCodename = codename };
        }

        public static ContentItemIdentifier ByExternalId(string externalId)
        {
            return new ContentItemIdentifier() { ItemExternalId = externalId };
        }
    }
}
