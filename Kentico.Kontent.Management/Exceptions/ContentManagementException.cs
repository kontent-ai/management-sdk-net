using System;
using System.Net;
using System.Net.Http;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Exceptions
{
    /// <summary>
    /// Represents an error response from the Kentico Kontent Content Management API.
    /// </summary>
    public sealed class ManagementException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the error message from the response.
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementException"/> class with information from an error response.
        /// </summary>
        /// <param name="response">The unsuccessful response.</param>
        /// <param name="exceptionMessage">The error response.</param>
        public ManagementException(HttpResponseMessage response, string exceptionMessage)
        {
            StatusCode = response.StatusCode;

            try
            {
                var errorModel = JObject.Parse(exceptionMessage).ToObject<ErrorResponseModel>();
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
                Message = $"Unknown error. HTTP status code: {StatusCode}. Reason phrase: {response.ReasonPhrase}.";
            }
        }
    }
}
