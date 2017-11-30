using KenticoCloud.ContentManagement.Models.Identifiers;
using System;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public sealed class AssetIdentifier : IdentifierWithExternalId<AssetIdentifier>
    {
        [Obsolete("For internal purposes. Use static method AssetIdenfifier.ByXYZ instead.")]
        public AssetIdentifier() { }
    }
}
