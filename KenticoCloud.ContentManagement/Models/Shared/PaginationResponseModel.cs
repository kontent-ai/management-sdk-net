using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models
{
    /// <summary>
    /// Represents the pagination response model.
    /// </summary>
    public sealed class PaginationResponseModel
    {
        /// <summary>
        /// Gets or sets continuation token.
        /// </summary>
        [JsonProperty("continuation_token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets next page.
        /// </summary>
        [JsonProperty("next_page")]
        public string NextPage { get; set; }
    }
}
