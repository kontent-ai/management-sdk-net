using Kontent.Ai.Management.Annotations;
using System.Collections.Concurrent;
using System.Reflection;

namespace Kontent.Ai.Management.Conversion;

/// <summary>
/// Resolves Kontent.ai content-type ids to the CLR record types that carry the matching
/// <see cref="KontentTypeAttribute"/>. Used by <see cref="ContentItemEnvelopeConverter"/> for polymorphic
/// dispatch on the read path — specifically, materializing rich-text components into their concrete generated
/// record types.
/// </summary>
/// <remarks>
/// Thread-safe; safe to share across calls. Population is lazy — a registry will scan an assembly on
/// first <see cref="Scan"/>, then cache the result and ignore repeated requests for the same assembly.
/// </remarks>
internal sealed class ContentTypeRegistry
{
    private readonly ConcurrentDictionary<string, Type> _byId = new(StringComparer.OrdinalIgnoreCase);

    // The read path resolves components by id (see ResolveById); this index only enforces codename uniqueness today.
    // It is retained deliberately: codename is the portable identity, so resolving an environment's component-type id
    // to a model via a fetched id↔codename map would key through here. Don't drop it as unused.
    private readonly ConcurrentDictionary<string, Type> _byCodename = new();
    private readonly ConcurrentDictionary<Assembly, byte> _scannedAssemblies = new();

    /// <summary>
    /// Indexes every <see cref="IElementsModel"/>-implementing type in <paramref name="assembly"/> that carries
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
    /// Registers <paramref name="type"/> if it implements <see cref="IElementsModel"/> and carries
    /// <see cref="KontentTypeAttribute"/>. Throws when a different type is already registered for the same codename.
    /// </summary>
    public void Register(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!TryRegister(type))
        {
            throw new ArgumentException(
                $"Type '{type.FullName}' is not a content-type record (must implement IElementsModel and carry [KontentType]).",
                nameof(type));
        }
    }

    /// <summary>
    /// Registers <paramref name="type"/> if it is a content-type record. Idempotent for the same type, and a
    /// non-content-type is a no-op so the read path can self-register a root without failing. A codename already
    /// mapped to a different type still throws — a duplicate codename is always a conflict.
    /// </summary>
    public void EnsureRegistered(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        TryRegister(type);
    }

    /// <summary>
    /// Resolves a content-type <paramref name="id"/> (from <see cref="KontentTypeAttribute.Id"/>) to the registered
    /// CLR type, or <c>null</c> if none is registered for it. Case-insensitive.
    /// </summary>
    public Type? ResolveById(string id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return _byId.TryGetValue(id, out var type) ? type : null;
    }

    private bool TryRegister(Type type)
    {
        var attr = type.GetCustomAttribute<KontentTypeAttribute>();
        if (attr is null || !typeof(IElementsModel).IsAssignableFrom(type)) return false;

        var existingByCodename = _byCodename.GetOrAdd(attr.Codename, type);
        if (existingByCodename != type)
        {
            throw new InvalidOperationException(
                $"Codename '{attr.Codename}' is already registered to type '{existingByCodename.FullName}'; cannot also register '{type.FullName}'.");
        }

        if (attr.Id is not null)
        {
            var existingById = _byId.GetOrAdd(attr.Id, type);
            if (existingById != type)
            {
                throw new InvalidOperationException(
                    $"Type id '{attr.Id}' is already registered to type '{existingById.FullName}'; cannot also register '{type.FullName}'.");
            }
        }

        return true;
    }
}
