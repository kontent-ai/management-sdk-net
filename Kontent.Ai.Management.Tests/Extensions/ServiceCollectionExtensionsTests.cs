using AwesomeAssertions;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Kontent.Ai.Management.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    private static readonly string ValidEnvironmentId = Guid.NewGuid().ToString();
    private const string ValidApiKey = "dummy-key";

    [Fact]
    public void AddManagementClient_DuplicateName_Throws()
    {
        var services = new ServiceCollection();
        services.AddManagementClient("production", ConfigureValidOptions);

        Action act = () => services.AddManagementClient("production", ConfigureValidOptions);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*already been registered*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("name with spaces")]
    [InlineData(" leading")]
    [InlineData("trailing ")]
    [InlineData(null)]
    public void AddManagementClient_InvalidName_Throws(string? name)
    {
        var services = new ServiceCollection();

        Action act = () => services.AddManagementClient(name!, ConfigureValidOptions);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddManagementClient_Default_RegistersUnkeyedAndKeyedClient()
    {
        var services = new ServiceCollection();
        services.AddManagementClient(ConfigureValidOptions);

        using var provider = services.BuildServiceProvider();

        var unkeyed = provider.GetRequiredService<IManagementClient>();
        var keyed = provider.GetRequiredKeyedService<IManagementClient>(ManagementClientNames.Default);
        var fromFactory = provider.GetRequiredService<IManagementClientFactory>().Get();

        unkeyed.Should().BeSameAs(keyed);
        fromFactory.Should().BeSameAs(unkeyed);
    }

    [Fact]
    public void AddManagementClient_NamedClient_ResolvableByFactoryAndKey()
    {
        var services = new ServiceCollection();
        services.AddManagementClient("alt", options =>
        {
            options.EnvironmentId = Guid.NewGuid().ToString();
            options.ApiKey = "alt-key";
        });

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IManagementClientFactory>();

        var fromKey = provider.GetRequiredKeyedService<IManagementClient>("alt");
        var fromFactory = factory.Get("alt");

        fromKey.Should().BeSameAs(fromFactory);
    }

    [Fact]
    public void AddManagementClient_DefaultAndNamed_AreIndependent()
    {
        var services = new ServiceCollection();
        services.AddManagementClient(ConfigureValidOptions);
        services.AddManagementClient("alt", options =>
        {
            options.EnvironmentId = Guid.NewGuid().ToString();
            options.ApiKey = "alt-key";
        });

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IManagementClientFactory>();

        factory.Get().Should().NotBeSameAs(factory.Get("alt"));
    }

    [Fact]
    public void Factory_Get_UnregisteredName_Throws()
    {
        var services = new ServiceCollection();
        services.AddManagementClient(ConfigureValidOptions);

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IManagementClientFactory>();

        Action act = () => factory.Get("missing");

        act.Should().Throw<InvalidOperationException>().WithMessage("*No management client registered with name 'missing'*");
    }

    [Fact]
    public void AddManagementClient_WithConfigurationSection_BindsOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Management:EnvironmentId"] = ValidEnvironmentId,
                ["Management:ApiKey"] = ValidApiKey,
                ["Management:EnableResilience"] = "false"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddManagementClient(configuration.GetSection("Management"));

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptionsMonitor<ManagementOptions>>().CurrentValue;

        options.EnvironmentId.Should().Be(ValidEnvironmentId);
        options.ApiKey.Should().Be(ValidApiKey);
        options.EnableResilience.Should().BeFalse();
    }

    [Fact]
    public void AddManagementClient_RegistersManagementApiAndSubscriptionApi()
    {
        var services = new ServiceCollection();
        services.AddManagementClient(ConfigureValidOptions);

        using var provider = services.BuildServiceProvider();

        provider.GetService<IManagementApi>().Should().NotBeNull();
        provider.GetService<ISubscriptionApi>().Should().NotBeNull();
        provider.GetRequiredKeyedService<IManagementApi>(ManagementClientNames.Default).Should().NotBeNull();
        provider.GetRequiredKeyedService<ISubscriptionApi>(ManagementClientNames.Default).Should().NotBeNull();
    }

    private static void ConfigureValidOptions(ManagementOptions options)
    {
        options.EnvironmentId = ValidEnvironmentId;
        options.ApiKey = ValidApiKey;
    }
}
