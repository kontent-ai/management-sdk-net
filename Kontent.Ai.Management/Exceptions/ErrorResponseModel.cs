using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Exceptions;

internal class ErrorResponseModel
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("validation_errors")]
    public IEnumerable<ValidationErrorModel> ValidationErrors { get; set; }
}
