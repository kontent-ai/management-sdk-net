namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Identifies a generated record as the .NET projection of a Kontent.ai content type. The <see cref="Codename"/>
/// is the canonical key used in API write requests; <see cref="Id"/> is optional and primarily aids debugging.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class KontentContentTypeAttribute(string codename, string? id = null) : Attribute
{
    /// <summary>Content type codename.</summary>
    public string Codename { get; } = codename ?? throw new ArgumentNullException(nameof(codename));

    /// <summary>Content type identifier (GUID), or <c>null</c> when not emitted.</summary>
    public string? Id { get; } = id;
}
