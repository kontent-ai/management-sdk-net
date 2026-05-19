using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Exceptions;

internal class ValidationErrorModel
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
