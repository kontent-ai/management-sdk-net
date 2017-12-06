using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of the sitemap node.
    /// </summary>
    public sealed class SitemapNodeIdentifier : IdentifierWithCodename<SitemapNodeIdentifier>
    {
        /// <summary>
        /// Constructor for internal use only. 
        /// Use static method SitemapNodeIdentifier.ByXYZ instead.
        /// </summary>
        [Obsolete("For internal purposes. Use static method SitemapNodeIdentifier.ByXYZ instead.")]
        public SitemapNodeIdentifier() { }
    }
}
