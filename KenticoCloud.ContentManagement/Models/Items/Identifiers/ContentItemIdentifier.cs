using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of content item.
    /// </summary>
    public sealed class ContentItemIdentifier : IdentifierWithExternalIdAndCodename<ContentItemIdentifier>
    {
        /// <summary>
        /// Constructor for internal use only. 
        /// Use static method AssetIdenfifier.ByXYZ instead.
        /// </summary>
        [Obsolete("For internal purposes. Use static method ContentItemIdentifier.ByXYZ instead.")]
        public ContentItemIdentifier() { }
    }
}
