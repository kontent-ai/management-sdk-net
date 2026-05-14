using Microsoft.Extensions.Options;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Honest domain abstraction over "where does the current <see cref="ManagementOptions"/> come from" so the same
/// handler / client code serves both the DI path (reactive, via <see cref="IOptionsMonitor{TOptions}"/>) and the
/// standalone path (captured snapshot) without an ad-hoc bridge. Deliberately NOT an <c>IOptionsMonitor</c> wrapper —
/// the standalone impl makes no claim to be reactive.
/// </summary>
internal interface IManagementOptionsAccessor
{
    ManagementOptions Current { get; }
}

internal sealed class MonitorBackedManagementOptionsAccessor(
    IOptionsMonitor<ManagementOptions> monitor,
    string optionsName) : IManagementOptionsAccessor
{
    public ManagementOptions Current => monitor.Get(optionsName);
}

internal sealed class SnapshotManagementOptionsAccessor(ManagementOptions snapshot) : IManagementOptionsAccessor
{
    private readonly ManagementOptions _snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));

    public ManagementOptions Current => _snapshot;
}
