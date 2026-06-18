namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Restricts the content types permitted in a linked-items, subpages, or rich-text element.
/// </summary>
/// <remarks>
/// The local validator can enforce this for rich-text components, where component type information is available
/// in memory. Linked-item and subpage references carry only item identifiers, so the MAPI remains the source of
/// truth for those constraints.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class AllowedTypesAttribute : Attribute
{
    /// <summary>Allowed content type codenames.</summary>
    public IReadOnlyList<string> Codenames { get; }

    /// <inheritdoc/>
    public AllowedTypesAttribute(params string[] codenames)
    {
        ArgumentNullException.ThrowIfNull(codenames);
        Codenames = codenames;
    }
}
