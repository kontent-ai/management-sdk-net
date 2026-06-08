using Kontent.Ai.Management.Annotations;
using System.Collections.Concurrent;
using System.Reflection;

namespace Kontent.Ai.Management.Conversion;

/// <summary>
/// Resolves Kontent.ai content-type codenames to the CLR record types that carry the matching
/// <see cref="KontentTypeAttribute"/>. Used by <see cref="ContentItemEnvelopeConverter"/> for polymorphic
/// dispatch on the read path — specifically, materializing rich-text components into their concrete
/// generated record types.
/// </summary>
/// <remarks>
/// Thread-safe; safe to share across calls. Population is lazy — a registry will scan an assembly on
/// first <see cref="Scan"/>, then cache the result and ignore repeated requests for the same assembly.
/// </remarks>
internal sealed class ContentTypeRegistry
{
    private readonly ConcurrentDictionary<string, Type> _byCodename = new();
    private readonly ConcurrentDictionary<Assembly, byte> _scannedAssemblies = new();

    /// <summary>
    /// Indexes every <see cref="IContentItem"/>-implementing type in <paramref name="assembly"/> that carries
    /// a <see cref="KontentTypeAttribute"/>. Idempotent — subsequent calls for the same assembly are no-ops.
    /// </summary>
    public void Scan(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        if (!_scannedAssemblies.TryAdd(assembly, 0)) return;

        foreach (var type in assembly.GetTypes())
        {
            TryRegister(type);
        }
    }

    /// <summary>
    /// Registers <paramref name="type"/> if it implements <see cref="IContentItem"/> and carries
    /// <see cref="KontentTypeAttribute"/>. Throws when a different type is already registered for the same codename.
    /// </summary>
    public void Register(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!TryRegister(type))
        {
            throw new ArgumentException(
                $"Type '{type.FullName}' is not a content-type record (must implement IContentItem and carry [KontentType]).",
                nameof(type));
        }
    }

    /// <summary>
    /// Resolves <paramref name="codename"/> to the registered CLR type, or returns <c>null</c> if no type is registered.
    /// </summary>
    public Type? Resolve(string codename)
    {
        ArgumentNullException.ThrowIfNull(codename);
        return _byCodename.TryGetValue(codename, out var type) ? type : null;
    }

    private bool TryRegister(Type type)
    {
        var attr = type.GetCustomAttribute<KontentTypeAttribute>();
        if (attr is null || !typeof(IContentItem).IsAssignableFrom(type)) return false;

        var existing = _byCodename.GetOrAdd(attr.Codename, type);
        if (existing != type)
        {
            throw new InvalidOperationException(
                $"Codename '{attr.Codename}' is already registered to type '{existing.FullName}'; cannot also register '{type.FullName}'.");
        }
        return true;
    }
}
