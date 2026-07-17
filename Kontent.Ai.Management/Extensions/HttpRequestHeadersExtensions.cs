using Kontent.Ai.Management.Attributes;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kontent.Ai.Management.Extensions;

internal static class HttpRequestHeadersExtensions
{
    private const string SdkTrackingHeaderName = "X-KC-SDKID";
    private const string SourceTrackingHeaderName = "X-KC-SOURCE";
    private const string PackageRepositoryHost = "nuget.org";

    private static readonly Lazy<string> Sdk = new(GetSdk);
    private static readonly Lazy<string?> Source = new(GetSource);

    // Resilience retries re-dispatch the same HttpRequestMessage instance, so header writes must be idempotent —
    // a plain Add would accumulate one duplicate value per attempt.
    internal static void AddSdkTrackingHeader(this HttpRequestHeaders headers)
    {
        headers.Remove(SdkTrackingHeaderName);
        headers.Add(SdkTrackingHeaderName, Sdk.Value);
    }

    internal static void AddSourceTrackingHeader(this HttpRequestHeaders headers)
    {
        var source = Source.Value;
        if (source is not null)
        {
            headers.Remove(SourceTrackingHeaderName);
            headers.Add(SourceTrackingHeaderName, source);
        }
    }

    // Reads the SemVer informational version and drops the build-metadata suffix ('+...'), which deterministic /
    // SourceLink builds append as the commit hash — it does not belong in a tracking header. Pre-release suffixes
    // ('-rc.1') are kept. Falls back to "0.0.0" for unversioned (local / dev) builds.
    internal static string GetProductVersion(this Assembly assembly)
    {
        var informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        return StripBuildMetadata(informationalVersion) ?? "0.0.0";
    }

    private static string? StripBuildMetadata(string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            return null;
        }

        var plusIndex = version.IndexOf('+', StringComparison.Ordinal);
        return plusIndex < 0 ? version : version[..plusIndex];
    }

    private static string GetSdk()
    {
        var assembly = typeof(ManagementClient).Assembly;
        return $"{PackageRepositoryHost};{assembly.GetName().Name};{assembly.GetProductVersion()}";
    }

    // Must never throw: the result is cached in a Lazy, so a thrown exception would be cached and rethrown on
    // every subsequent request for the process lifetime — hence the blanket catch; the header is simply omitted.
    private static string? GetSource()
    {
        try
        {
            var originatingAssembly = GetOriginatingAssembly();
            if (originatingAssembly is null)
            {
                return null;
            }

            var attribute = originatingAssembly.GetCustomAttributes<SourceTrackingHeaderAttribute>().FirstOrDefault();
            return attribute is null ? null : GenerateSourceTrackingHeaderValue(originatingAssembly, attribute);
        }
        catch
        {
            return null;
        }
    }

    private static string GenerateSourceTrackingHeaderValue(Assembly originatingAssembly, SourceTrackingHeaderAttribute attribute)
    {
        if (attribute.LoadFromAssembly)
        {
            var packageName = attribute.PackageName ?? originatingAssembly.GetName().Name;
            return $"{packageName};{originatingAssembly.GetProductVersion()}";
        }

        var preRelease = attribute.PreReleaseLabel is null ? "" : $"-{attribute.PreReleaseLabel}";
        return $"{attribute.PackageName};{attribute.MajorVersion}.{attribute.MinorVersion}.{attribute.PatchVersion}{preRelease}";
    }

    // Best-effort: walks the synchronous call stack for the outermost assembly that references this SDK, so a
    // package built on the SDK can be attributed via its [assembly: SourceTrackingHeader]. Returns null when no
    // such frame is present (e.g. first invoked from an async continuation) — the source header is then simply
    // omitted.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly? GetOriginatingAssembly()
    {
        var executingAssembly = typeof(ManagementClient).Assembly;

        return new StackTrace().GetFrames()
            .Select(frame => frame.GetMethod()?.ReflectedType?.Assembly)
            .Distinct()
            .OfType<Assembly>()
            .LastOrDefault(assembly => assembly.GetReferencedAssemblies().Any(referenced => referenced.FullName == executingAssembly.FullName));
    }
}
