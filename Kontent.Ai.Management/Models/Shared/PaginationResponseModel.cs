
namespace Kontent.Ai.Management.Models.Shared;

internal sealed record PaginationResponseModel
{
    [JsonPropertyName("continuation_token")]
    public string? Token { get; init; }

    [JsonPropertyName("next_page")]
    public string? NextPage { get; init; }
}
