namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Identifies a property as the .NET projection of a single content type element. The codename is what the
/// SDK writes on outbound envelopes; the id is what the API uses on inbound envelopes — the converter maps
/// between the two using this attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class KontentElementAttribute(string codename, string id) : Attribute
{
    /// <summary>Element codename. Used as the canonical key for outbound writes and validator error reporting.</summary>
    public string Codename { get; } = codename ?? throw new ArgumentNullException(nameof(codename));

    /// <summary>Element identifier (GUID). Used to look up the property when deserializing inbound responses.</summary>
    public string Id { get; } = id ?? throw new ArgumentNullException(nameof(id));
}
