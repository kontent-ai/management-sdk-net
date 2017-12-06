using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents asset identifier.
    /// </summary>
    public sealed class AssetIdentifier : IdentifierWithExternalId<AssetIdentifier>
    {
        /// <summary>
        /// Constructor for internal use only. 
        /// Use static method AssetIdenfifier.ByXYZ instead.
        /// </summary>
        [Obsolete("For internal purposes. Use static method AssetIdenfifier.ByXYZ instead.")]
        public AssetIdentifier() { }
    }
}
