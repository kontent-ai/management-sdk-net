namespace Kontent.Ai.Management.Configuration;

internal static class ManagementOptionsExtensions
{
    /// <summary>Composes the versioned, scoped base address — <c>{Endpoint}/v2/{scopePath}</c> — for a Refit client.</summary>
    public static Uri ScopedEndpoint(this ManagementOptions options, string scopePath) =>
        new($"{options.Endpoint.TrimEnd('/')}/v2/{scopePath}", UriKind.Absolute);
}
