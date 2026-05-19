namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Maximum permitted size of any single asset referenced by the property, in bytes. The validator records the
/// constraint but does not enforce it — the referenced asset's byte size is server-side state, checkable only
/// when upload context is available.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class MaxAssetSizeAttribute(long bytes) : Attribute
{
    /// <summary>Upper bound in bytes (inclusive).</summary>
    public long Bytes { get; } = bytes;
}
