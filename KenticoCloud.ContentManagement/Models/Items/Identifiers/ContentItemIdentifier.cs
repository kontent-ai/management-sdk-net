using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemIdentifier : IdentifierWithExternalIdAndCodename<ContentItemIdentifier>
    {
        [Obsolete("For internal purposes. Use static method ContentItemIdentifier.ByXYZ instead.")]
        public ContentItemIdentifier() { }
    }
}
