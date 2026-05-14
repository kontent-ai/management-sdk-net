namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Restricts the content types permitted as inline item links inside a rich-text element. Distinct from
/// <see cref="AllowedTypesAttribute"/>, which on rich text covers components and direct linked-item references.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class AllowedItemLinkTypesAttribute : Attribute
{
    /// <summary>Allowed content type codenames for inline item links.</summary>
    public IReadOnlyList<string> Codenames { get; }

    public AllowedItemLinkTypesAttribute(params string[] codenames)
    {
        ArgumentNullException.ThrowIfNull(codenames);
        Codenames = codenames;
    }
}
