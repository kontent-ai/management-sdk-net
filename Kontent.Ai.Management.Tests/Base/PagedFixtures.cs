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
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().GetAt(0).Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();
}
