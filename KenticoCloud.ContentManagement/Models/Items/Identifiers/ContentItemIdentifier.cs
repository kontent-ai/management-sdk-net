using System;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemIdentifier
    {
        public Guid Id { get; private set; }
        public string Codename { get; private set; }
        public string ExternalId { get; private set; }

        public static ContentItemIdentifier ById(Guid id) {
            return new ContentItemIdentifier() { Id = id };
        }

        public static ContentItemIdentifier ByCodename(string codename)
        {
            return new ContentItemIdentifier() { Codename = codename };
        }

        public static ContentItemIdentifier ByExternalId(string externalId)
        {
            return new ContentItemIdentifier() { ExternalId = externalId };
        }
    }
}
