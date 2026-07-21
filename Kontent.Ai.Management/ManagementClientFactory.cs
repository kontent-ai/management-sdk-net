using Kontent.Ai.Management.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kontent.Ai.Management;

internal sealed class ManagementClientFactory(IServiceProvider serviceProvider) : IManagementClientFactory
{
    public IManagementClient Get() => Get(ManagementClientNames.Default);

    public IManagementClient Get(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        try
        {
            return serviceProvider.GetRequiredKeyedService<IManagementClient>(name);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(
                $"No management client registered with name '{name}'. Ensure you've registered the client using AddManagementClient(\"{name}\", ...).",
                ex);
        }
    }
}
