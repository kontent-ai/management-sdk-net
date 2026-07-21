
namespace Kontent.Ai.Management.Models.Shared;

internal sealed record PaginationResponseModel
{
    [JsonPropertyName("continuation_token")]
    public string? Token { get; init; }
}
