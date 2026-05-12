using Kontent.Ai.Management.Models.Shared;

namespace Kontent.Ai.Management.Api;

internal static class ReferenceUrlExtensions
{
    /// <summary>
    /// Renders a <see cref="Reference"/> as the path segment the Management API expects after a resource collection:
    /// the bare <c>id</c>, <c>codename/{codename}</c>, or <c>external-id/{externalId}</c>. When more than one is set,
    /// the id wins, then the codename — matching <see cref="Reference.ToDynamic"/>.
    /// </summary>
    public static string ToUrlSegment(this Reference reference)
    {
        ArgumentNullException.ThrowIfNull(reference);

        return reference switch
        {
            { Id: { } id } => id.ToString(),
            { Codename: { } codename } => $"codename/{Uri.EscapeDataString(codename)}",
            { ExternalId: { } externalId } => $"external-id/{Uri.EscapeDataString(externalId)}",
            _ => throw new ArgumentException("Reference must have an id, codename, or external id set.", nameof(reference)),
        };
    }
}
