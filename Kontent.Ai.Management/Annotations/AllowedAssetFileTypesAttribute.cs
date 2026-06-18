using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Restricts the file types permitted in an asset element, mirroring the MAPI's <c>allowed_file_types</c>. Schema
/// metadata only; the MAPI enforces it at upsert time and the client does not check it locally. The default
/// (<see cref="FileType.Any"/>) means no constraint and is typically not emitted.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class AllowedAssetFileTypesAttribute(FileType types) : Attribute
{
    /// <summary>Allowed file-type bucket.</summary>
    public FileType Types { get; } = types;
}
