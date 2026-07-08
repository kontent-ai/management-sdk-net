namespace Kontent.Ai.Management.Configuration;

internal static class ManagementOptionsExtensions
{
    internal const string SubscriptionIdMissingMessage =
        "SubscriptionId is not configured. Set ManagementOptions.SubscriptionId to call subscription endpoints.";

    /// <summary>Composes the versioned, scoped base address — <c>{Endpoint}/v2/{scopePath}</c> — for a Refit client.</summary>
    public static Uri ScopedEndpoint(this ManagementOptions options, string scopePath) =>
        new($"{options.Endpoint.TrimEnd('/')}/v2/{scopePath}", UriKind.Absolute);

    public static bool HasSubscriptionId(this ManagementOptions options) =>
        !string.IsNullOrWhiteSpace(options.SubscriptionId);

    /// <summary>The subscription scope path; throws a descriptive error when <see cref="ManagementOptions.SubscriptionId"/> is not configured.</summary>
    public static string SubscriptionScopePath(this ManagementOptions options) =>
        options.HasSubscriptionId()
            ? $"subscriptions/{options.SubscriptionId}"
            : throw new InvalidOperationException(SubscriptionIdMissingMessage);
}
