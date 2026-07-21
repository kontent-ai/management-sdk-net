namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Identifies an enum member as the .NET projection of a multiple-choice element option. Each option's
/// codename and id are preserved so the envelope converter can round-trip between the wire shape
/// (id or codename references) and the strongly-typed enum.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class KontentEnumValueAttribute(string codename, string id) : Attribute
{
    /// <summary>Option codename. Used as the canonical key for outbound writes.</summary>
    public string Codename { get; } = codename ?? throw new ArgumentNullException(nameof(codename));

    /// <summary>Option identifier (GUID). Used to resolve the enum member when reading API responses.</summary>
    public string Id { get; } = id ?? throw new ArgumentNullException(nameof(id));
}
