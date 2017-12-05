using System;
using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class TaxonomyTermIdentifier : Identifier<TaxonomyTermIdentifier>
    {
        [Obsolete("For internal purposes. Use static method TaxonomyTermIdentifier.ByXYZ instead.")]
        public TaxonomyTermIdentifier() { }
    }
}
