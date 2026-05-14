namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Maximum permitted size of any single asset referenced by the property, in bytes. Phase 3's validator records
/// the constraint but does not enforce it — the referenced asset's size is server-side state. The constraint is
/// surfaced by the envelope converter (phase 4 onward) when upload context is available.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class MaxAssetSizeAttribute(long bytes) : Attribute
{
    /// <summary>Upper bound in bytes (inclusive).</summary>
    public long Bytes { get; } = bytes;
}
