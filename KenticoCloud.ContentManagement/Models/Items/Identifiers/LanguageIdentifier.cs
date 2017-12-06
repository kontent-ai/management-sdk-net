using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of the language.
    /// </summary>
    public sealed class LanguageIdentifier : IdentifierWithCodename<LanguageIdentifier>
    {
        /// <summary>
        /// Constructor for internal use only. 
        /// Use static method LanguageIdentifier.ByXYZ instead.
        /// </summary>
        [Obsolete("For internal purposes. Use static method LanguageIdentifier.ByXYZ instead.")]
        public LanguageIdentifier() { }

        /// <summary>
        /// Identifier for default language.
        /// </summary>
        public static readonly LanguageIdentifier DEFAULT_LANGUAGE = LanguageIdentifier.ById(Guid.Empty);
    }
}
