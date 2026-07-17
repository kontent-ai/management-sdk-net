using System.Text.Json;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Maps Refit <see cref="IApiResponse"/> values onto <see cref="IManagementResult"/>.
/// </summary>
internal static class RefitApiResponseExtensions
{
    private const int MaxRawBodyLength = 500;

    public static Task<IManagementResult<T>> ToManagementResultAsync<T>(this IApiResponse<T> response) =>
        response.ToManagementResultAsync(static value => value);

    public static async Task<IManagementResult<T>> ToManagementResultAsync<T>(this Task<IApiResponse<T>> responseTask) =>
        await (await responseTask.ConfigureAwait(false)).ToManagementResultAsync().ConfigureAwait(false);

    public static async Task<IManagementResult<TValue>> ToManagementResultAsync<TResponse, TValue>(
        this Task<IApiResponse<TResponse>> responseTask,
        Func<TResponse, TValue> selector) =>
        await (await responseTask.ConfigureAwait(false)).ToManagementResultAsync(selector).ConfigureAwait(false);

    public static async Task<IManagementResult> ToManagementResultAsync(this Task<IApiResponse> responseTask) =>
        await (await responseTask.ConfigureAwait(false)).ToManagementResultAsync().ConfigureAwait(false);

    // Disposing after mapping is safe on both paths: Refit fully buffers the response (the deserialized Content and
    // the ApiException's body-as-string outlive the HttpResponseMessage), and without it every response message
    // lingers until finalization.
    public static async Task<IManagementResult<TValue>> ToManagementResultAsync<TResponse, TValue>(
        this IApiResponse<TResponse> response,
        Func<TResponse, TValue> selector)
    {
        using (response)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.Content is null)
                {
                    return ManagementResult<TValue>.Failure(
                        new Error
                        {
                            Message = "The Management API returned a success status but no readable response body.",
                            Exception = response.Error,
                        },
                        response.StatusCode,
                        RequestUrl(response));
                }

                return ManagementResult<TValue>.Success(
                    selector(response.Content),
                    response.StatusCode,
                    RequestUrl(response));
            }

            return await MapFailureAsync<TValue>(response).ConfigureAwait(false);
        }
    }

    public static async Task<IManagementResult> ToManagementResultAsync(this IApiResponse response)
    {
        using (response)
        {
            if (response.IsSuccessStatusCode)
            {
                return ManagementResult.Success(
                    response.StatusCode,
                    RequestUrl(response));
            }

            return await MapFailureAsync(response).ConfigureAwait(false);
        }
    }

    private static async Task<IManagementResult<TValue>> MapFailureAsync<TValue>(IApiResponse response)
    {
        var error = await BuildErrorAsync(response).ConfigureAwait(false);
        return ManagementResult<TValue>.Failure(error, response.StatusCode, RequestUrl(response));
    }

    private static async Task<IManagementResult> MapFailureAsync(IApiResponse response)
    {
        var error = await BuildErrorAsync(response).ConfigureAwait(false);
        return ManagementResult.Failure(error, response.StatusCode, RequestUrl(response));
    }

    private static async Task<IError> BuildErrorAsync(IApiResponse response)
    {
        var apiException = response.Error;
        if (apiException is null)
        {
            return new Error();
        }

        try
        {
            var parsed = await apiException.GetContentAsAsync<Error>().ConfigureAwait(false);
            return parsed is not null
                ? parsed with { Exception = apiException }
                : new Error { Message = apiException.Message, Exception = apiException };
        }
        catch (JsonException)
        {
            // The body was not a Management API error envelope — an HTML 5xx page, plain text, or empty.
            var rawBody = apiException.Content;
            var message = string.IsNullOrWhiteSpace(rawBody)
                ? apiException.Message
                : $"{apiException.Message} | Raw response: {Truncate(rawBody)}";

            return new Error { Message = message, Exception = apiException };
        }
    }

    private static string? RequestUrl(IApiResponse response) =>
        response.RequestMessage?.RequestUri?.ToString();

    private static string Truncate(string body) =>
        body.Length > MaxRawBodyLength ? body[..MaxRawBodyLength] + "... (truncated)" : body;
}
