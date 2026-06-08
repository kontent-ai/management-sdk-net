using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Handlers;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using System.ComponentModel.DataAnnotations;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API. Implements <see cref="IDisposable"/> /
/// <see cref="IAsyncDisposable"/> so non-DI consumers can release the underlying <see cref="HttpClient"/> instances;
/// DI-managed instances pass <c>null</c> for <c>ownedResources</c> and Dispose becomes a no-op.
/// </summary>
public sealed partial class ManagementClient : IManagementClient, IAsyncDisposable
{
    private const int MAX_FILE_SIZE_MB = 100;

    private readonly IManagementApi _managementApi;
    private readonly ISubscriptionApi _subscriptionApi;
    private readonly IDisposable _ownedResources;
    private readonly Conversion.ContentItemEnvelopeConverter _contentConverter;
    // When we built the converter ourselves, auto-scan the consumer's generated-models assembly on first use so
    // rich-text component types resolve. When one was injected (tests / advanced callers), trust its registry as-is.
    private readonly bool _autoScanContentTypes;
    private int _disposed;

    /// <summary>
    /// Creates a client against the environment described by <paramref name="managementOptions"/>. The returned
    /// instance owns its <see cref="HttpClient"/>s — dispose it when you're done. Throws
    /// <see cref="ValidationException"/> if the options fail validation. For DI scenarios prefer
    /// <c>services.AddManagementClient(...)</c>, which hands lifetime management to the container.
    /// </summary>
    public ManagementClient(ManagementOptions managementOptions)
        : this(managementOptions, configureResilience: null, configureRefit: null)
    {
    }

    // Standalone construction with the optional hooks the ManagementClientBuilder exposes. The public ctor above
    // delegates here with nulls, so its behaviour is unchanged.
    internal ManagementClient(
        ManagementOptions managementOptions,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience,
        Action<RefitSettings>? configureRefit)
    {
        var (api, subscriptionApi, ownedResources) = BuildDependencies(managementOptions, configureResilience, configureRefit);

        _managementApi = api;
        _subscriptionApi = subscriptionApi;
        _ownedResources = ownedResources;
        _contentConverter = new Conversion.ContentItemEnvelopeConverter();
        _autoScanContentTypes = true;
    }

    internal ManagementClient(
        IManagementApi managementApi,
        ISubscriptionApi subscriptionApi,
        IDisposable? ownedResources = null,
        Conversion.ContentItemEnvelopeConverter? contentConverter = null)
    {
        _managementApi = managementApi;
        _subscriptionApi = subscriptionApi;
        _ownedResources = ownedResources;
        _contentConverter = contentConverter ?? new Conversion.ContentItemEnvelopeConverter();
        _autoScanContentTypes = contentConverter is null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _ownedResources?.Dispose();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        if (_ownedResources is IAsyncDisposable async)
        {
            await async.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _ownedResources?.Dispose();
        }
    }

    // Builds the env-scoped and subscription-scoped Refit clients plus the disposable bundle the ctor needs.
    // Validates options here so all standalone construction paths surface ValidationException uniformly (the DI
    // path uses ValidateOnStart, a separate mechanism with its own exception type — that's not something we control).
    private static (IManagementApi Api, ISubscriptionApi SubscriptionApi, IDisposable OwnedResources) BuildDependencies(
        ManagementOptions options,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience,
        Action<RefitSettings>? configureRefit)
    {
        ArgumentNullException.ThrowIfNull(options);
        Validator.ValidateObject(options, new ValidationContext(options), validateAllProperties: true);

        var optionsAccessor = new SnapshotManagementOptionsAccessor(options);
        var pipeline = BuildResiliencePipeline(options, configureResilience);
        var refitSettings = Configuration.RefitSettingsProvider.CreateRefitSettings(configureRefit);

        var managementHttp = BuildHttpClient(options, $"projects/{options.EnvironmentId}", pipeline, optionsAccessor);
        var subscriptionHttp = BuildHttpClient(options, $"subscriptions/{options.SubscriptionId}", pipeline, optionsAccessor);

        var api = RestService.For<IManagementApi>(managementHttp, refitSettings);
        var subscriptionApi = RestService.For<ISubscriptionApi>(subscriptionHttp, refitSettings);

        return (api, subscriptionApi, new CompositeDisposable(managementHttp, subscriptionHttp));
    }

    private static ResiliencePipeline<HttpResponseMessage> BuildResiliencePipeline(
        ManagementOptions options,
        Action<ResiliencePipelineBuilder<HttpResponseMessage>>? configureResilience)
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();
        if (options.EnableResilience)
        {
            // Mirrors the DI path's ConfigureResilienceHandler: a supplied hook fully replaces the default pipeline.
            if (configureResilience is not null)
            {
                configureResilience(builder);
            }
            else
            {
                ServiceCollectionExtensions.ConfigureDefaultResilience(builder);
            }
        }
        return builder.Build();
    }

    private static HttpClient BuildHttpClient(
        ManagementOptions options,
        string scopePath,
        ResiliencePipeline<HttpResponseMessage> pipeline,
        IManagementOptionsAccessor optionsAccessor)
    {
        // Innermost → outermost: primary → resilience → auth → tracking. Matches the DI pipeline's order so each
        // retry re-runs auth + tracking fresh.
        HttpMessageHandler primary = new HttpClientHandler();
        var resilience = new ResilienceHandler(pipeline) { InnerHandler = primary };
        var auth = new ManagementAuthenticationHandler(optionsAccessor) { InnerHandler = resilience };
        var tracking = new TrackingHandler { InnerHandler = auth };

        return new HttpClient(tracking)
        {
            BaseAddress = new Uri(string.Format(options.EndpointV2, scopePath), UriKind.Absolute),
        };
    }

    private sealed class CompositeDisposable(params IDisposable[] items) : IDisposable, IAsyncDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
            foreach (var item in items)
            {
                item.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
            foreach (var item in items)
            {
                if (item is IAsyncDisposable a)
                {
                    await a.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    item.Dispose();
                }
            }
        }
    }
}
