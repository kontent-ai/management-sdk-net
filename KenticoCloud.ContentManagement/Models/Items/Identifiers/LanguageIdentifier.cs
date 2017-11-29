using System;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class LanguageIdentifier
    {
        public Guid Id { get; private set; }
        public string Codename { get; private set; }

        public static LanguageIdentifier ById(Guid id)
        {
            return new LanguageIdentifier() { Id = id };
        }

        public static LanguageIdentifier ByCodename(string codename)
        {
            return new LanguageIdentifier() { Codename = codename };
        }
    }
}
