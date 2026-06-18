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
        if (response.IsSuccessStatusCode)
        {
            return Task.FromResult<IManagementResult<TValue>>(ManagementResult<TValue>.Success(
                selector(response.Content!),
                response.StatusCode,
                RequestUrl(response)));
        }

        return MapFailureAsync<TValue>(response);
    }

    public static Task<IManagementResult> ToManagementResultAsync(this IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Task.FromResult<IManagementResult>(ManagementResult.Success(
                response.StatusCode,
                RequestUrl(response)));
        }

        return MapFailureAsync(response);
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
            return new Error { Message = "Unknown error." };
        }

        try
        {
            var parsed = await apiException.GetContentAsAsync<Error>().ConfigureAwait(false);
            return parsed is not null
                ? parsed with { Exception = apiException }
                : new Error { Message = apiException.Message, Exception = apiException };
        }
        catch (Exception)
        {
            // The body was not a Management API error envelope — an HTML 5xx page, plain text, or empty.
            var rawBody = apiException.Content;
            var message = string.IsNullOrWhiteSpace(rawBody)
                ? apiException.Message
                : $"{apiException.Message} | Raw response: {Truncate(rawBody)}";

            return new Error { Message = message, Exception = apiException };
        }
    }

    private static string RequestUrl(IApiResponse response) =>
        response.RequestMessage?.RequestUri?.ToString() ?? string.Empty;

    private static string Truncate(string body) =>
        body.Length > MaxRawBodyLength ? body[..MaxRawBodyLength] + "... (truncated)" : body;
}
