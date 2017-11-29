using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Exceptions
{
    internal class ErrorResponseModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("validation_errors")]
        public IEnumerable<ValidationErrorModel> ValidationErrors { get; set; }
    }
}
