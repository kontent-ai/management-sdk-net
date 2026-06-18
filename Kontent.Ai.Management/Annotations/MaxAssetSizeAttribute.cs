namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Maximum permitted size of any single asset referenced by the property, in bytes. The in-memory record carries
/// only asset identifiers, so the MAPI remains the source of truth for this constraint.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class MaxAssetSizeAttribute(long bytes) : Attribute
{
    /// <summary>Upper bound in bytes (inclusive).</summary>
    public long Bytes { get; } = bytes;
}
