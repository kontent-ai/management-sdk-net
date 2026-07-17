using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.Base;

internal static class PagedFixtures
{
    /// <summary>
    /// Builds the expected list for a paged listing test: each page fixture is a single-property envelope
    /// whose value is the page's item array; those arrays are deserialized and concatenated in order.
    /// Pass a single page for a non-paged listing.
    /// </summary>
    public static List<T> ConcatPages<T>(params string[] pages)
        => pages.SelectMany(DeserializePageItems<T>).ToList();

    private static List<T> DeserializePageItems<T>(string page)
    {
        var envelope = JsonNode.Parse(page)!.AsObject();
        var itemProperties = envelope.Where(property => property.Key != "pagination").ToList();
        if (itemProperties.Count != 1)
        {
            throw new InvalidOperationException(
                $"A page fixture must contain exactly one items property besides \"pagination\", but this one has: {string.Join(", ", envelope.Select(property => $"\"{property.Key}\""))}.");
        }

        return JsonSerializer.Deserialize<List<T>>(itemProperties[0].Value!.ToString(), SharedTestJsonOptions.Default)!;
    }
}
