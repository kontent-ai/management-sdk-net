using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Registers the Kontent.ai Management client and its dependencies on an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
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
    /// <param name="configureRefit">Optional hook to tweak Refit settings (still Newtonsoft-backed until model regen).</param>
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

    private static void RegisterRefitClient<T>(
        IServiceCollection services,
        string clientName,
        string httpClientName,
        Func<ManagementOptions, string> scopePathSelector,
        RefitSettings refitSettings,
        Action<IHttpClientBuilder>? configureHttpClient,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience) where T : class
    {
        var httpClientBuilder = services
            .AddHttpClient(httpClientName)
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<ManagementOptions>>().Get(clientName);
                httpClient.BaseAddress = new Uri(string.Format(options.EndpointV2, scopePathSelector(options)), UriKind.Absolute);
            });

        // Resilience first → resilience sits outermost so each retry re-runs tracking + auth fresh (matters when
        // tokens rotate). Diverges from delivery/sync by omitting AddTimeout — see ConfigureDefaultResilience.
        ConfigureResilienceHandler(httpClientBuilder, $"management_{typeof(T).Name}_{clientName}", clientName, configureResilience);
        AddMessageHandlers(httpClientBuilder, clientName);
        configureHttpClient?.Invoke(httpClientBuilder);

        services.AddKeyedTransient<T>(clientName, (sp, _) =>
        {
            var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName);
            return RestService.For<T>(httpClient, refitSettings);
        });
    }

    private static RefitSettings CreateRefitSettings(Action<RefitSettings>? configureRefit)
        => RefitSettingsProvider.CreateRefitSettings(configureRefit);

    private static void ConfigureResilienceHandler(
        IHttpClientBuilder httpClientBuilder,
        string resilienceHandlerName,
        string clientName,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience)
    {
        httpClientBuilder.AddResilienceHandler(resilienceHandlerName, (builder, context) =>
        {
            var options = context.ServiceProvider.GetRequiredService<IOptionsMonitor<ManagementOptions>>().Get(clientName);

            if (!options.EnableResilience)
            {
                return;
            }

            if (configureResilience is not null)
            {
                configureResilience(builder);
            }
            else
            {
                ConfigureDefaultResilience(builder);
            }
        });
    }

    private static void AddMessageHandlers(IHttpClientBuilder httpClientBuilder, string clientName)
    {
        httpClientBuilder.AddHttpMessageHandler(_ => new TrackingHandler());
        httpClientBuilder.AddHttpMessageHandler(sp => new ManagementAuthenticationHandler(
            new MonitorBackedManagementOptionsAccessor(
                sp.GetRequiredService<IOptionsMonitor<ManagementOptions>>(),
                clientName)));
    }

    private static IManagementClient CreateManagementClient(IServiceProvider serviceProvider, object? key)
    {
        var name = (string)key!;
        var managementApi = serviceProvider.GetRequiredKeyedService<IManagementApi>(name);
        var subscriptionApi = serviceProvider.GetRequiredKeyedService<ISubscriptionApi>(name);
        var options = serviceProvider.GetRequiredService<IOptionsMonitor<ManagementOptions>>().Get(name);
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

    /// <summary>
    /// Configures the default resilience pipeline. Diverges from delivery-sdk-net / sync-sdk-net by omitting
    /// <c>AddTimeout</c> — management operations include asset uploads where a per-attempt timeout would be more
    /// hindrance than help. Consumers who want one should add it via the <c>configureResilience</c> hook on
    /// <c>AddManagementClient</c>.
    /// </summary>
    internal static void ConfigureDefaultResilience(ResiliencePipelineBuilder<HttpResponseMessage> builder)
    {
        builder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            ShouldHandle = args => ValueTask.FromResult(
                IsTransientException(args.Outcome.Exception, args.Context.CancellationToken) ||
                (args.Outcome.Result?.IsSuccessStatusCode == false &&
                 IsRetryableStatusCode(args.Outcome.Result?.StatusCode))),
            DelayGenerator = GetRetryAfterDelay
        });
    }

    internal static ValueTask<TimeSpan?> GetRetryAfterDelay(RetryDelayGeneratorArguments<HttpResponseMessage> args)
    {
        if (args.Outcome.Result is { StatusCode: System.Net.HttpStatusCode.TooManyRequests } response
            && response.Headers.RetryAfter?.Delta is { } retryAfter)
        {
            return ValueTask.FromResult<TimeSpan?>(retryAfter);
        }

        return ValueTask.FromResult<TimeSpan?>(null);
    }

    internal static bool IsRetryableStatusCode(System.Net.HttpStatusCode? statusCode)
        => statusCode is
            System.Net.HttpStatusCode.TooManyRequests or
            System.Net.HttpStatusCode.RequestTimeout or
            System.Net.HttpStatusCode.InternalServerError or
            System.Net.HttpStatusCode.BadGateway or
            System.Net.HttpStatusCode.ServiceUnavailable or
            System.Net.HttpStatusCode.GatewayTimeout;

    internal static bool IsTransientException(Exception? exception, CancellationToken requestCancellationToken)
    {
        if (exception is null)
        {
            return false;
        }

        if (exception is OperationCanceledException)
        {
            if (requestCancellationToken.IsCancellationRequested)
            {
                return false;
            }

            return exception is TaskCanceledException || exception.InnerException is TimeoutException;
        }

        return exception is HttpRequestException or TimeoutException;
    }
}
