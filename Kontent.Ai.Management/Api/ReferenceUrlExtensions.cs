using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Api;

internal static class ReferenceUrlExtensions
{
    /// <summary>
    /// Renders a <see cref="Reference"/> as the path segment the Management API expects after a resource collection:
    /// the bare <c>id</c>, <c>codename/{codename}</c>, or <c>external-id/{externalId}</c>. The codename / external id
    /// are validated (see <see cref="EnsureSingleSegment"/>) but not escaped: they route through a Refit <c>{**}</c>
    /// catch-all that percent-encodes reserved data characters itself, so pre-escaping here would double-encode
    /// (a space would reach the wire as <c>%2520</c>).
    /// </summary>
    public static string ToUrlSegment(this Reference reference)
    {
        ArgumentNullException.ThrowIfNull(reference);

        return reference switch
        {
            { Id: { } id } => id.ToString(),
            { Codename: { } codename } => $"codename/{EnsureSingleSegment(codename)}",
            { ExternalId: { } externalId } => $"external-id/{EnsureSingleSegment(externalId)}",
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
    /// <c>email/{email}</c>. The id / email are validated (see <see cref="EnsureSingleSegment"/>) but not escaped —
    /// the <c>{**}</c> catch-all percent-encodes reserved data characters itself.
    /// </summary>
    public static string ToUrlSegment(this UserIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return identifier switch
        {
            { Id: { } id } => EnsureSingleSegment(id),
            { Email: { } email } => $"email/{EnsureSingleSegment(email)}",
            _ => throw new ArgumentException("You must provide user id or email.", nameof(identifier)),
        };
    }

    /// <summary>
    /// Guards a caller-supplied identifier value that is interpolated into a Refit <c>{**}</c> catch-all route.
    /// The catch-all percent-encodes reserved data characters (<c>?</c>, <c>#</c>, <c>%</c>, spaces, <c>\</c>, control
    /// characters) but passes <c>/</c> and the dot-segments <c>.</c> / <c>..</c> through as path structure — so an
    /// unchecked value could retarget the request to a different Management API endpoint under the same token (the
    /// surrounding <c>codename/</c> / <c>email/</c> prefix supplies the leading slash even for a lone <c>..</c>).
    /// These are the only characters that survive the catch-all as structure; rejecting them closes the traversal.
    /// </summary>
    private static string EnsureSingleSegment(string value)
    {
        if (value.Contains('/') || value.Contains('\\') || value is "." or "..")
        {
            throw new ArgumentException(
                $"'{value}' is not a valid identifier: it must not contain '/' or '\\', or be '.' or '..'.", nameof(value));
        }

        return value;
    }
}
