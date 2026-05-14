namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Mirrors the MAPI's <c>allowed_file_types</c> enum on an asset element. The validator surfaces this constraint
/// on outbound writes when upload context is available (phase 4 onward); phase 3's validator records but does
/// not enforce it.
/// </summary>
public enum AssetFileType
{
    /// <summary>No restriction (equivalent to omitting the attribute).</summary>
    Any,

    /// <summary>The Kontent.ai "adjustable" file-type bucket (image/video, etc.).</summary>
    Adjustable,

    /// <summary>Images only.</summary>
    Image,
}

/// <summary>
/// Restricts the file types permitted in an asset element. Pair with <see cref="AssetFileType"/>; the default
/// (<see cref="AssetFileType.Any"/>) means no constraint and is typically not emitted.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class AllowedAssetFileTypesAttribute(AssetFileType types) : Attribute
{
    /// <summary>Allowed file-type bucket.</summary>
    public AssetFileType Types { get; } = types;
}
