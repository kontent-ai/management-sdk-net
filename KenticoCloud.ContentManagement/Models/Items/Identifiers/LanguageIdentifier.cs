using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class LanguageIdentifier : Identifier<LanguageIdentifier>
    {
        [Obsolete("For internal purposes. Use static method LanguageIdentifier.ByXYZ instead.")]
        public LanguageIdentifier() { }
    }
}
