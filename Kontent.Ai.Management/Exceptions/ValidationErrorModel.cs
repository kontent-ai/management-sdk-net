using Newtonsoft.Json;

namespace Kontent.Ai.Management.Exceptions;

internal class ValidationErrorModel
{
    [JsonProperty("message")]
    public string Message { get; set; }
}
