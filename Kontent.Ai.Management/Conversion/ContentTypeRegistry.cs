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
    /// Registers <paramref name="type"/>. Throws when it is not a content-type record (must implement
    /// <see cref="IElementsModel"/> and carry <see cref="KontentTypeAttribute"/> with an id), or when a
    /// different type is already registered for the same id.
    /// </summary>
    public void Register(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.GetCustomAttribute<KontentTypeAttribute>() is not { } attr || !typeof(IElementsModel).IsAssignableFrom(type))
        {
            throw new ArgumentException(
                $"Type '{type.FullName}' is not a content-type record (must implement IElementsModel and carry [KontentType]).",
                nameof(type));
        }
        if (attr.Id is null)
        {
            throw new ArgumentException(
                $"Type '{type.FullName}' carries [KontentType] without an id, so it can never be resolved as a rich-text component. " +
                "Set the attribute's id to register it.",
                nameof(type));
        }

        AddById(type, attr.Id);
    }

    /// <summary>
    /// Registers <paramref name="type"/> if it is a content-type record. Idempotent for the same type, and a
    /// non-content-type is a no-op so the read path can self-register a root without failing. An id already
    /// mapped to a different type still throws — two types claiming one id cannot both be resolved.
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

    private void TryRegister(Type type)
    {
        // Id-less content-type records are skipped, not rejected: only the component read path needs ids, and
        // scans / read-path self-registration must tolerate write-only models that never carry one.
        if (type.GetCustomAttribute<KontentTypeAttribute>() is { Id: { } id } && typeof(IElementsModel).IsAssignableFrom(type))
        {
            AddById(type, id);
        }
    }

    private void AddById(Type type, string id)
    {
        var existingById = _byId.GetOrAdd(id, type);
        if (existingById != type)
        {
            throw new InvalidOperationException(
                $"Type id '{id}' is already registered to type '{existingById.FullName}'; cannot also register '{type.FullName}'.");
        }
    }
}
