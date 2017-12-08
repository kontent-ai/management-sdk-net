using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models
{
    internal sealed class PaginationResponseModel
    {
        [JsonProperty("continuation_token")]
        public string Token { get; set; }

        [JsonProperty("next_page")]
        public string NextPage { get; set; }
    }
}
