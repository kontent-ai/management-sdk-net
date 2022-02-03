using Kentico.Kontent.Management.Models.Shared;

namespace Kentico.Kontent.Management.Modules.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Reference"/>.
    /// </summary>
    public static class ReferenceExtensions
    {
        internal static bool DoesHaveOnlyIdSet(this Reference property) =>
            property.Id != null && property.Codename == null && property.ExternalId == null;
        internal static bool DoesHaveOnlyCodenameSet(this Reference property) =>
            property.Id == null && property.Codename != null && property.ExternalId == null;
        internal static bool DoesHaveOnlyExternalIdSet(this Reference property) =>
            property.Id == null && property.Codename == null && property.ExternalId != null;
        
        /// <summary>
        /// Check whether <see cref="Reference"/> is identified by only one identifier.
        /// </summary>
        public static bool DoesHaveSetOnlyOneIdentifier(this Reference property)
        {
            return property.DoesHaveOnlyIdSet() || property.DoesHaveOnlyCodenameSet() || property.DoesHaveOnlyExternalIdSet();
        }
    }
}