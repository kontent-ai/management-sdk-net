using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Kontent.Ai.Management.Exceptions;

/// <summary>
/// Represents an error response from the Kontent.ai Management API.
/// </summary>
public sealed class ManagementException : Exception
{
    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Gets the error message from the response.
    /// </summary>
    public override string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementException"/> class with information from an error response.
    /// </summary>
    /// <param name="response">The unsuccessful response.</param>
    /// <param name="exceptionMessage">The error response body.</param>
    public ManagementException(HttpResponseMessage response, string exceptionMessage)
        : this(response.StatusCode, response.ReasonPhrase, exceptionMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementException"/> class with information from an error response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    /// <param name="reasonPhrase">The HTTP reason phrase of the response.</param>
    /// <param name="exceptionMessage">The error response body.</param>
    public ManagementException(HttpStatusCode statusCode, string? reasonPhrase, string exceptionMessage)
    {
        StatusCode = statusCode;

        try
        {
            var errorModel = JsonSerializer.Deserialize<ErrorResponseModel>(exceptionMessage, Configuration.RefitSettingsProvider.CreateDefaultJsonSerializerOptions());
            var message = errorModel.Message;

            if (errorModel.ValidationErrors != null)
            {
                var errors = string.Join(Environment.NewLine, errorModel.ValidationErrors.Select(error => error.Message));

                message += $"{Environment.NewLine}Validation errors:{Environment.NewLine}{errors}";
            }

            Message = message;
        }
        catch (Exception)
        {
            Message = $"Unknown error. HTTP status code: {StatusCode}. Reason phrase: {reasonPhrase}.";
        }
    }
}
