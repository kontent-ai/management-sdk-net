using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of the content type.
    /// </summary>
    public sealed class ContentTypeIdentifier : IdentifierWithCodename<ContentTypeIdentifier>
    {
        /// <summary>
        /// Constructor for internal use only. 
        /// Use static method ContentTypeIdentifier.ByXYZ instead.
        /// </summary>
        [Obsolete("For internal purposes. Use static method ContentTypeIdentifier.ByXYZ instead.")]
        public ContentTypeIdentifier() { }
    }
}
