using System;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentVariantIdentifier
    {
        public Guid LanguageId { get; private set; }
        public string LanguageCodename { get; private set; }

        public static ContentVariantIdentifier ByLanguageId(Guid languageId)
        {
            return new ContentVariantIdentifier() { LanguageId = languageId };
        }

        public static ContentVariantIdentifier ByLanguageCodename(string languageCodename)
        {
            return new ContentVariantIdentifier() { LanguageCodename = languageCodename };
        }
    }
}
