using System.Net;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Maps Refit <see cref="IApiResponse"/> values onto <see cref="IManagementResult"/>.
/// </summary>
internal static class RefitApiResponseExtensions
{
    private const int MaxRawBodyLength = 500;

    public static Task<IManagementResult<T>> ToManagementResultAsync<T>(this IApiResponse<T> response) =>
        response.ToManagementResultAsync(static value => value);

    public static Task<IManagementResult<TValue>> ToManagementResultAsync<TResponse, TValue>(
        this IApiResponse<TResponse> response,
        Func<TResponse, TValue> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (response.IsSuccessStatusCode)
        {
            return Task.FromResult<IManagementResult<TValue>>(ManagementResult<TValue>.Success(
                selector(response.Content!),
                response.StatusCode,
                RequestUrl(response),
                response.Headers));
        }

        return MapFailureAsync<TValue>(response);
    }

    public static Task<IManagementResult> ToManagementResultAsync(this IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Task.FromResult<IManagementResult>(ManagementResult.Success(
                response.StatusCode,
                RequestUrl(response),
                response.Headers));
        }

        return MapFailureAsync(response);
    }

    private static async Task<IManagementResult<TValue>> MapFailureAsync<TValue>(IApiResponse response)
    {
        var error = await BuildErrorAsync(response).ConfigureAwait(false);
        return ManagementResult<TValue>.Failure(error, StatusCodeOf(response), RequestUrl(response), response.Headers);
    }

    private static async Task<IManagementResult> MapFailureAsync(IApiResponse response)
    {
        var error = await BuildErrorAsync(response).ConfigureAwait(false);
        return ManagementResult.Failure(error, StatusCodeOf(response), RequestUrl(response), response.Headers);
    }

    private static async Task<IError> BuildErrorAsync(IApiResponse response)
    {
        var apiException = response.Error;
        if (apiException is null)
        {
            return new Error { Message = "Unknown error." };
        }

        try
        {
            var parsed = await apiException.GetContentAsAsync<Error>().ConfigureAwait(false);
            return parsed is not null
                ? parsed with { Exception = apiException }
                : new Error { Message = apiException.Message, Exception = apiException };
        }
        catch (Exception parseException) when (!IsFatalException(parseException))
        {
            // The body was not a Management API error envelope — an HTML 5xx page, plain text, or empty.
            var rawBody = apiException.Content;
            var message = string.IsNullOrWhiteSpace(rawBody)
                ? apiException.Message
                : $"{apiException.Message} | Raw response: {Truncate(rawBody)}";

            return new Error { Message = message, Exception = apiException };
        }
    }

    private static HttpStatusCode? StatusCodeOf(IApiResponse response) =>
        response.Error is not null ? response.StatusCode : null;

    private static string RequestUrl(IApiResponse response) =>
        response.RequestMessage?.RequestUri?.ToString() ?? string.Empty;

    private static string Truncate(string body) =>
        body.Length > MaxRawBodyLength ? body[..MaxRawBodyLength] + "... (truncated)" : body;

    private static bool IsFatalException(Exception exception) =>
        exception is OutOfMemoryException
            or StackOverflowException
            or AccessViolationException
            or AppDomainUnloadedException
            or BadImageFormatException
            or CannotUnloadAppDomainException
            or InvalidProgramException;
}
