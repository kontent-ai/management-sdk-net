namespace Kontent.Ai.Management;

/// <summary>
/// Resolves named <see cref="IManagementClient"/> instances registered via <c>AddManagementClient(name, ...)</c>.
/// </summary>
public interface IManagementClientFactory
{
    /// <summary>
    /// Returns the default-named management client.
    /// </summary>
    IManagementClient Get();

    /// <summary>
    /// Returns the management client registered under <paramref name="name"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">No client with the given name has been registered.</exception>
    IManagementClient Get(string name);
}
