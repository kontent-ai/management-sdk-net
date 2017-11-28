using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Exceptions
{
    internal class ValidationErrorModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
