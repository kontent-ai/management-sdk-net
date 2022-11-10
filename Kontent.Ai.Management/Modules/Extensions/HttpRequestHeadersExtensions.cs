using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;

namespace Kontent.Ai.Management.Modules.Extensions;

internal static class HttpRequestHeadersExtensions
{
    private const string SdkTrackingHeaderName = "X-KC-SDKID";

    private const string PackageRepositoryHost = "nuget.org";

    private static readonly Lazy<string> SdkVersion = new(GetSdkVersion);
    private static readonly Lazy<string> SdkPackageId = new(GetSdkPackageId);


    internal static void AddSdkTrackingHeader(this HttpRequestHeaders header) => header.Add(SdkTrackingHeaderName, GetSdkTrackingHeader());

    internal static string GetSdkTrackingHeader() => $"{PackageRepositoryHost};{SdkPackageId.Value};{SdkVersion.Value}";

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
}
