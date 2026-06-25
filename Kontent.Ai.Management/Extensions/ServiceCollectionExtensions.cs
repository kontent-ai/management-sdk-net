using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Registers the Kontent.ai Management client and its dependencies on an <see cref="IServiceCollection"/>.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    private const string ManagementHttpClientPrefix = "Kontent.Ai.Management.HttpClient.";
    private const string SubscriptionHttpClientPrefix = "Kontent.Ai.Management.SubscriptionHttpClient.";

    /// <summary>
    /// Registers the management client using an <see cref="IConfiguration"/> section. Defaults to section
    /// <c>"ManagementOptions"</c> if the name is omitted.
    /// </summary>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationSectionName = "ManagementOptions")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var section = string.IsNullOrWhiteSpace(configurationSectionName)
            ? configuration
            : configuration.GetSection(configurationSectionName);

        return services.AddManagementClientFromConfiguration(ManagementClientNames.Default, section);
    }

    /// <summary>
    /// Registers a named management client using an <see cref="IConfiguration"/> section.
    /// </summary>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        string name,
        IConfiguration configuration,
        string configurationSectionName = "ManagementOptions")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var section = string.IsNullOrWhiteSpace(configurationSectionName)
            ? configuration
            : configuration.GetSection(configurationSectionName);

        return services.AddManagementClientFromConfiguration(name, section);
    }

    /// <summary>
    /// Registers the management client using an <see cref="IConfigurationSection"/>.
    /// </summary>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurationSection);

        return services.AddManagementClientFromConfiguration(ManagementClientNames.Default, configurationSection);
    }

    /// <summary>
    /// Registers a named management client using an <see cref="IConfigurationSection"/>.
    /// </summary>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        string name,
        IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurationSection);

        return services.AddManagementClientFromConfiguration(name, configurationSection);
    }

    /// <summary>
    /// Registers the default management client with an options-configuration action.
    /// </summary>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        Action<ManagementOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);

        return services.AddManagementClient(ManagementClientNames.Default, configureOptions);
    }

    /// <summary>
    /// Registers the default management client with options + HTTP/resilience customisation.
    /// </summary>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        Action<ManagementOptions> configureOptions,
        Action<IHttpClientBuilder>? configureHttpClient,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience = null)
    {
        return services.AddManagementClient(
            ManagementClientNames.Default,
            configureOptions,
            configureHttpClient,
            configureResilience);
    }

    /// <summary>
    /// Registers a named management client. This is the primary overload all others delegate to.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="name">Client name. Must be unique across all registrations.</param>
    /// <param name="configureOptions">Action to configure the management options.</param>
    /// <param name="configureHttpClient">Optional hook on the underlying <see cref="IHttpClientBuilder"/> (both env and subscription clients).</param>
    /// <param name="configureResilience">Optional hook to replace the default resilience pipeline.</param>
    /// <param name="configureRefit">Optional hook to tweak Refit settings.</param>
    /// <exception cref="InvalidOperationException">A client with the same name is already registered.</exception>
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        string name,
        Action<ManagementOptions> configureOptions,
        Action<IHttpClientBuilder>? configureHttpClient = null,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience = null,
        Action<RefitSettings>? configureRefit = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ValidateClientName(name);
        ArgumentNullException.ThrowIfNull(configureOptions);

        EnsureClientNameNotAlreadyRegistered(services, name);

        services.Configure(name, configureOptions);
        services.AddOptions<ManagementOptions>(name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        if (name == ManagementClientNames.Default)
        {
            services.Configure(configureOptions);
            services.AddOptions<ManagementOptions>()
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        return CompleteClientRegistration(services, name, configureHttpClient, configureResilience, configureRefit);
    }

    private static IServiceCollection AddManagementClientFromConfiguration(
        this IServiceCollection services,
        string name,
        IConfiguration configuration,
        Action<IHttpClientBuilder>? configureHttpClient = null,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience = null,
        Action<RefitSettings>? configureRefit = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ValidateClientName(name);
        ArgumentNullException.ThrowIfNull(configuration);

        EnsureClientNameNotAlreadyRegistered(services, name);

        services.Configure<ManagementOptions>(name, configuration);
        services.AddOptions<ManagementOptions>(name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        if (name == ManagementClientNames.Default)
        {
            services.Configure<ManagementOptions>(configuration);
            services.AddOptions<ManagementOptions>()
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        return CompleteClientRegistration(services, name, configureHttpClient, configureResilience, configureRefit);
    }

    private static IServiceCollection CompleteClientRegistration(
        IServiceCollection services,
        string name,
        Action<IHttpClientBuilder>? configureHttpClient,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience,
        Action<RefitSettings>? configureRefit)
    {
        var refitSettings = CreateRefitSettings(configureRefit);

        RegisterRefitClient<IManagementApi>(
            services,
            name,
            $"{ManagementHttpClientPrefix}{name}",
            o => $"projects/{o.EnvironmentId}",
            refitSettings,
            configureHttpClient,
            configureResilience);

        RegisterRefitClient<ISubscriptionApi>(
            services,
            name,
            $"{SubscriptionHttpClientPrefix}{name}",
            o => $"subscriptions/{o.SubscriptionId}",
            refitSettings,
            configureHttpClient,
            configureResilience);

        services.AddKeyedSingleton<IManagementClient>(name, CreateManagementClient);
        services.TryAddSingleton<IManagementClientFactory, ManagementClientFactory>();

        if (name == ManagementClientNames.Default)
        {
            services.TryAddSingleton(sp => sp.GetRequiredKeyedService<IManagementClient>(ManagementClientNames.Default));
            services.TryAddSingleton(sp => sp.GetRequiredKeyedService<IManagementApi>(ManagementClientNames.Default));
            services.TryAddSingleton(sp => sp.GetRequiredKeyedService<ISubscriptionApi>(ManagementClientNames.Default));
        }

        return services;
    }

    private static IManagementClient CreateManagementClient(IServiceProvider serviceProvider, object? key)
    {
        var name = (string)key!;
        var managementApi = serviceProvider.GetRequiredKeyedService<IManagementApi>(name);
        var subscriptionApi = serviceProvider.GetRequiredKeyedService<ISubscriptionApi>(name);
        return new ManagementClient(managementApi, subscriptionApi);
    }

    private static void ValidateClientName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Trim() != name || name.Contains(' '))
        {
            throw new ArgumentException(
                "Client name cannot contain leading/trailing whitespace, or contain spaces. Use underscores or hyphens instead.",
                nameof(name));
        }
    }

    private static void EnsureClientNameNotAlreadyRegistered(IServiceCollection services, string name)
    {
        if (!services.Any(d => d.ServiceType == typeof(IManagementClient) && Equals(d.ServiceKey, name)))
        {
            return;
        }

        throw new InvalidOperationException(
            $"A management client with the name '{name}' has already been registered. Each client must have a unique name.");
    }
}
