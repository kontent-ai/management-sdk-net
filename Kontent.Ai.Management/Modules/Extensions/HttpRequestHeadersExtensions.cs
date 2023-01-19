using Kontent.Ai.Management.Attributes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kontent.Ai.Management.Modules.Extensions;

internal static class HttpRequestHeadersExtensions
{
    private const string SdkTrackingHeaderName = "X-KC-SDKID";
    private const string SourceTrackingHeaderName = "X-KC-SOURCE";

    private const string PackageRepositoryHost = "nuget.org";

    private static readonly Lazy<string> SdkVersion = new(GetSdkVersion);
    private static readonly Lazy<string> SdkPackageId = new(GetSdkPackageId);
    private static readonly Lazy<string> Source = new Lazy<string>(GetSource);


    internal static void AddSdkTrackingHeader(this HttpRequestHeaders header) => header.Add(SdkTrackingHeaderName, GetSdkTrackingHeader());

    internal static void AddSourceTrackingHeader(this HttpRequestHeaders headers)
    {
        var source = Source.Value;
        if (source != null)
        {
            headers.Add(SourceTrackingHeaderName, source);
        }
    }

    internal static bool RetryAfterExists(this HttpResponseHeaders headers)
        => headers?.RetryAfter?.Delta != null || headers?.RetryAfter?.Date != null;


    internal static TimeSpan GetRetryAfter(this HttpResponseHeaders headers)
    {
        static TimeSpan GetPositiveOrZero(TimeSpan timeSpan) => timeSpan < TimeSpan.Zero ? TimeSpan.Zero : timeSpan;

        if (headers?.RetryAfter?.Delta != null)
        {
            return GetPositiveOrZero(headers.RetryAfter.Delta.GetValueOrDefault(TimeSpan.Zero));
        }

        if (headers?.RetryAfter?.Date != null)
        {
            return GetPositiveOrZero(headers.RetryAfter.Date.Value - DateTime.UtcNow);
        }

        return TimeSpan.Zero;
    }

    private static string GetSdkTrackingHeader() => $"{PackageRepositoryHost};{SdkPackageId.Value};{SdkVersion.Value}";

    private static string GetSdkVersion()
    {
        var assembly = typeof(ManagementClient).Assembly;
        var sdkVersion = assembly
            .GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault()?.InformationalVersion;

        return sdkVersion;
    }

    private static string GetSdkPackageId()
    {
        var assembly = typeof(ManagementClient).Assembly;
        var sdkPackageId = assembly.GetName().Name;

        return sdkPackageId;
    }

    private static string GetSource()
    {
        Assembly originatingAssembly = GetOriginatingAssembly();

        var attribute = originatingAssembly.GetCustomAttributes<SourceTrackingHeaderAttribute>().FirstOrDefault();
        if (attribute != null)
        {
            return GenerateSourceTrackingHeaderValue(originatingAssembly, attribute);
        }
        return null;
    }

    private static string GetProductVersion(this Assembly assembly)
    {
        string sdkVersion;

        if (string.IsNullOrEmpty(assembly.Location))
        {
            // Assembly.Location can be empty when publishing to a single file
            // https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file
            sdkVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        }
        else
        {
            try
            {
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                sdkVersion = fileVersionInfo.ProductVersion;
            }
            catch (FileNotFoundException)
            {
                sdkVersion = "0.0.0";
            }
        }
        return sdkVersion ?? "0.0.0";
    }

    private static string GenerateSourceTrackingHeaderValue(Assembly originatingAssembly, SourceTrackingHeaderAttribute attribute)
    {
        string packageName;
        string version;
        if (attribute.LoadFromAssembly)
        {
            packageName = attribute.PackageName ?? originatingAssembly.GetName().Name;
            version = originatingAssembly.GetProductVersion();
        }
        else
        {
            packageName = attribute.PackageName;
            string preRelease = attribute.PreReleaseLabel == null ? "" : $"-{attribute.PreReleaseLabel}";
            version = $"{attribute.MajorVersion}.{attribute.MinorVersion}.{attribute.PatchVersion}{preRelease}";
        }
        return $"{packageName};{version}";
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly GetOriginatingAssembly()
    {
        var executingAssembly = typeof(ManagementClient).Assembly;

        var callerAssemblies = new StackTrace().GetFrames()
                    .Select(x => x.GetMethod().ReflectedType?.Assembly).Distinct().OfType<Assembly>()
                    .Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == executingAssembly.FullName));
        var originatingAssembly = callerAssemblies.Last();

        return originatingAssembly;
    }
}
