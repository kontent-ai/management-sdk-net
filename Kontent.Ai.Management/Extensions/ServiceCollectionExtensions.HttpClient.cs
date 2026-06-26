using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Kontent.Ai.Management.Extensions;

public static partial class ServiceCollectionExtensions
{
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
                httpClient.BaseAddress = options.ScopedEndpoint(scopePathSelector(options));
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
    {
        var settings = RefitSettingsProvider.CreateDefaultSettings();
        configureRefit?.Invoke(settings);
        return settings;
    }

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
        => exception switch
        {
            null => false,
            // A caller-initiated cancellation is not transient; a timeout-driven one (surfaced as a bare
            // TaskCanceledException or wrapping a TimeoutException) is.
            OperationCanceledException when requestCancellationToken.IsCancellationRequested => false,
            OperationCanceledException => exception is TaskCanceledException || exception.InnerException is TimeoutException,
            HttpRequestException or TimeoutException => true,
            _ => false,
        };
}
