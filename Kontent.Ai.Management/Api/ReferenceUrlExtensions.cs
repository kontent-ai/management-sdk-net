using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.LanguageVariants;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Which kinds of <see cref="Reference"/> an endpoint accepts in its URL. Mirrors the per-resource restrictions the
/// legacy URL templates encoded (e.g. webhooks accept only an id, spaces accept id or codename but not an external id).
/// </summary>
[Flags]
internal enum ReferenceKinds
{
    Id = 1,
    Codename = 2,
    ExternalId = 4,
    All = Id | Codename | ExternalId,
}

internal static class ReferenceUrlExtensions
{
    /// <summary>
    /// Renders a <see cref="Reference"/> as the path segment the Management API expects after a resource collection:
    /// the bare <c>id</c>, <c>codename/{codename}</c>, or <c>external-id/{externalId}</c>. When more than one is set,
    /// the id wins, then the codename. Throws if the reference uses a kind
    /// the endpoint doesn't support (see <paramref name="allowed"/>).
    /// </summary>
    public static string ToUrlSegment(this Reference reference, ReferenceKinds allowed = ReferenceKinds.All)
    {
        ArgumentNullException.ThrowIfNull(reference);

        return reference switch
        {
            { Id: { } id } when allowed.HasFlag(ReferenceKinds.Id) => id.ToString(),
            { Codename: { } codename } when allowed.HasFlag(ReferenceKinds.Codename) => $"codename/{Uri.EscapeDataString(codename)}",
            { ExternalId: { } externalId } when allowed.HasFlag(ReferenceKinds.ExternalId) => $"external-id/{Uri.EscapeDataString(externalId)}",
            { Id: not null } or { Codename: not null } or { ExternalId: not null } =>
                throw new InvalidOperationException("The provided identifier kind is not supported for this endpoint."),
            _ => throw new ArgumentException("Reference must have an id, codename, or external id set.", nameof(reference)),
        };
    }

    /// <summary>
    /// Renders a language variant identifier as <c>{item}/variants/{language}</c>. The language part supports id or
    /// codename only.
    /// </summary>
    public static string ToUrlSegment(this LanguageVariantIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return $"{identifier.ItemIdentifier.ToUrlSegment()}/variants/{identifier.LanguageIdentifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.Codename)}";
    }

    /// <summary>
    /// Renders an asset rendition identifier as <c>{asset}/renditions/{rendition}</c>. The rendition part supports id
    /// or external id only.
    /// </summary>
    public static string ToUrlSegment(this AssetRenditionIdentifier identifier)
    {
        ArgumentNullException.ThrowIfNull(identifier);

        return $"{identifier.AssetIdentifier.ToUrlSegment()}/renditions/{identifier.RenditionIdentifier.ToUrlSegment(ReferenceKinds.Id | ReferenceKinds.ExternalId)}";
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
