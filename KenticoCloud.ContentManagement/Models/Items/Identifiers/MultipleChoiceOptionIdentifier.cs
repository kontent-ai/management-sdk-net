using System;
using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class MultipleChoiceOptionIdentifier : Identifier<MultipleChoiceOptionIdentifier>
    {
        [Obsolete("For internal purposes. Use static method MultipleChoiceOptionIdentifier.ByXYZ instead.")]
        public MultipleChoiceOptionIdentifier() { }
    }
}
