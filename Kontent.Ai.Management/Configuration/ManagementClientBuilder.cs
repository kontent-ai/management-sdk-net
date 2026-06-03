using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Fluent, dependency-injection-free entry point for building an <see cref="IManagementClient"/>.
/// </summary>
/// <remarks>
/// <para>
/// The built client owns its underlying <see cref="HttpClient"/>s and is returned as a plain
/// <see cref="IManagementClient"/> — there is no container to reach into. Dispose it when you are done:
/// <c>await using var client = ManagementClientBuilder.WithOptions(...).Build();</c>.
/// </para>
/// <para>
/// This is a thin wrapper over the resource-owning <see cref="ManagementClient"/> constructor; it does not
/// spin up a private service provider. For applications using dependency injection, prefer
/// <c>services.AddManagementClient(...)</c>, which hands lifetime to the container and integrates with
/// <c>IHttpClientFactory</c>, options reloading, and named/keyed clients.
/// </para>
/// </remarks>
public sealed class ManagementClientBuilder
{
    private readonly ManagementOptions _options;
    private Action<ResiliencePipelineBuilder<HttpResponseMessage>>? _configureResilience;
    private Action<RefitSettings>? _configureRefit;

    private ManagementClientBuilder(ManagementOptions options) => _options = options;

    /// <summary>
    /// Starts a builder, configuring the options on a fresh <see cref="ManagementOptions"/> instance.
    /// </summary>
    /// <param name="configureOptions">Delegate that sets the required environment ID, API key, and any optional settings.</param>
    /// <returns>A builder for optional further configuration.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="configureOptions"/> is null.</exception>
    public static ManagementClientBuilder WithOptions(Action<ManagementOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);

        var options = new ManagementOptions();
        configureOptions(options);
        return new ManagementClientBuilder(options);
    }

    /// <summary>
    /// Starts a builder from a pre-built <see cref="ManagementOptions"/> instance (e.g. one bound from configuration).
    /// </summary>
    /// <param name="options">The management options.</param>
    /// <returns>A builder for optional further configuration.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
    public static ManagementClientBuilder WithOptions(ManagementOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new ManagementClientBuilder(options);
    }

    /// <summary>
    /// Replaces the default resilience pipeline with a custom one. Has no effect when
    /// <see cref="ManagementOptions.EnableResilience"/> is <c>false</c> (matching the DI behaviour).
    /// </summary>
    /// <param name="configureResilience">Delegate that configures the resilience pipeline.</param>
    /// <returns>The builder, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="configureResilience"/> is null.</exception>
    public ManagementClientBuilder WithResilience(Action<ResiliencePipelineBuilder<HttpResponseMessage>> configureResilience)
    {
        ArgumentNullException.ThrowIfNull(configureResilience);

        _configureResilience = configureResilience;
        return this;
    }

    /// <summary>
    /// Tweaks the Refit settings used by the client (e.g. to adjust the underlying serializer options).
    /// </summary>
    /// <param name="configureRefit">Delegate that mutates the <see cref="RefitSettings"/>.</param>
    /// <returns>The builder, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="configureRefit"/> is null.</exception>
    public ManagementClientBuilder ConfigureRefit(Action<RefitSettings> configureRefit)
    {
        ArgumentNullException.ThrowIfNull(configureRefit);

        _configureRefit = configureRefit;
        return this;
    }

    /// <summary>
    /// Builds a configured <see cref="IManagementClient"/> that owns its underlying <see cref="HttpClient"/>s.
    /// </summary>
    /// <returns>A new client. Dispose it (or <c>await using</c> it) to release the HTTP resources.</returns>
    /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">The options fail validation (e.g. missing or malformed environment ID or API key).</exception>
    public IManagementClient Build()
        => new ManagementClient(_options, _configureResilience, _configureRefit);
}
