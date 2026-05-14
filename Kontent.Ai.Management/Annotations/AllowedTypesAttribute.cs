namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Restricts the content types permitted in a linked-items, subpages, or rich-text element. The validator and
/// envelope converter use this to reject (or refuse to deserialize) items whose <c>[KontentContentType]</c>
/// codename is not in the allow-list.
/// </summary>
/// <remarks>
/// On rich-text properties, this covers both inline components and inline linked-item references — the MAPI does
/// not distinguish "allowed component types" from "allowed linked-item types" for rich text.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class AllowedTypesAttribute : Attribute
{
    /// <summary>Allowed content type codenames.</summary>
    public IReadOnlyList<string> Codenames { get; }

    public AllowedTypesAttribute(params string[] codenames)
    {
        ArgumentNullException.ThrowIfNull(codenames);
        Codenames = codenames;
    }
}
