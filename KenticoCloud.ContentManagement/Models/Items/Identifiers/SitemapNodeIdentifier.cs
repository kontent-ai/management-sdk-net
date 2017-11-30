using System;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    public sealed class SitemapNodeIdentifier : Identifier<SitemapNodeIdentifier>
    {
        [Obsolete("For internal purposes. Use static method SitemapNodeIdentifier.ByXYZ instead.")]
        public SitemapNodeIdentifier() { }
    }
}
