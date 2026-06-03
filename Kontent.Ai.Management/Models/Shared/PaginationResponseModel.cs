
namespace Kontent.Ai.Management.Models.Shared;

internal sealed class PaginationResponseModel
{
    [JsonPropertyName("continuation_token")]
    public string? Token { get; set; }

    [JsonPropertyName("next_page")]
    public string? NextPage { get; set; }
}
