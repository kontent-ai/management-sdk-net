using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Api;

internal static class ReferenceUrlExtensions
{
    /// <summary>
    /// Renders a <see cref="Reference"/> as the path segment the Management API expects after a resource collection:
    /// the bare <c>id</c>, <c>codename/{codename}</c>, or <c>external-id/{externalId}</c>. The codename / external id
    /// are left raw — they route through a Refit <c>{**}</c> catch-all that percent-encodes them once; pre-escaping
    /// here would double-encode (a space would reach the wire as <c>%2520</c>). A literal <c>/</c> in an external id
    /// cannot round-trip through the catch-all (it is treated as a path separator); codenames are <c>[a-z0-9_]</c> and
    /// unaffected.
    /// </summary>
    public static string ToUrlSegment(this Reference reference)
    {
        ArgumentNullException.ThrowIfNull(reference);

        return reference switch
        {
            { Id: { } id } => id.ToString(),
            { Codename: { } codename } => $"codename/{codename}",
            { ExternalId: { } externalId } => $"external-id/{externalId}",
            _ => throw new ArgumentException("Reference must have an id, codename, or external id set.", nameof(reference)),
        };
    }

    /// <summary>
    /// Renders a language variant identifier as <c>{item}/variants/{language}</c>.
    /// </summary>
    public static string ToUrlSegment(this LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return $"{identifier.ItemIdentifier.ToUrlSegment()}/variants/{identifier.LanguageIdentifier.ToUrlSegment()}";
    }

    /// <summary>
    /// Renders an asset rendition identifier as <c>{asset}/renditions/{rendition}</c>.
    /// </summary>
    public static string ToUrlSegment(this AssetRenditionIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return $"{identifier.AssetIdentifier.ToUrlSegment()}/renditions/{identifier.RenditionIdentifier.ToUrlSegment()}";
    }

    /// <summary>
    /// Renders a user identifier as the path segment the Management API expects: the bare <c>id</c>, or
    /// <c>email/{email}</c>. The email is left raw — it routes through a <c>{**}</c> catch-all that percent-encodes it.
    /// </summary>
    public static string ToUrlSegment(this UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return identifier switch
        {
            { Id: { } id } => id,
            { Email: { } email } => $"email/{email}",
            _ => throw new ArgumentException("You must provide user id or email.", nameof(identifier)),
        };
    }
}
