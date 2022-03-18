namespace Kentico.Kontent.Management.Modules.ResiliencePolicy;

internal enum RetryHttpCode
{
    RequestTimeout = 408,
    TooManyRequests = 429,
    InternalServerError = 500,
    BadGateway = 502,
    ServiceUnavailable = 503,
    GatewayTimeout = 504,
}
