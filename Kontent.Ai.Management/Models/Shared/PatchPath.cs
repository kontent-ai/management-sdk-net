namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Renders a <see cref="Reference"/> as a content-type / snippet PATCH <c>path</c> selector
/// (<c>id:{guid}</c>, <c>codename:{codename}</c>, or <c>external_id:{externalId}</c>). This is the
/// JSON-Pointer grammar of the patch body and is distinct from the REST route grammar produced by
/// <c>ReferenceUrlExtensions.ToUrlSegment</c>; the two are not interchangeable.
/// </summary>
internal static class PatchPath
{
    public static string Selector(Reference reference)
    {
        ArgumentNullException.ThrowIfNull(reference);

        return reference switch
        {
            { Id: { } id } => $"id:{id}",
            { Codename: { } codename } => $"codename:{codename}",
            { ExternalId: { } externalId } => $"external_id:{externalId.Replace("/", "\\/")}",
            _ => throw new ArgumentException("Reference must have an id, codename, or external id set.", nameof(reference)),
        };
    }
}
