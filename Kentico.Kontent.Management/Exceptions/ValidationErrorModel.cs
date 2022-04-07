using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Exceptions;

internal class ValidationErrorModel
{
    [JsonProperty("message")]
    public string Message { get; set; }
}
