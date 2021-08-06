using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Shared
{
    internal sealed class PaginationResponseModel
    {
        [JsonProperty("continuation_token")]
        public string Token { get; set; }

        [JsonProperty("next_page")]
        public string NextPage { get; set; }
    }
}
