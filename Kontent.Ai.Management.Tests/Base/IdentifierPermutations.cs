using Kontent.Ai.Management.Models.LanguageVariants;
using System.Collections;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Shared building blocks for the variant/rendition identifier permutation theories: the three reference kinds
/// (each paired with the URL segment it renders to) and a cartesian product over two axes. Each test composes its
/// own typed identifier and endpoint URL from the yielded reference pairs.
/// </summary>
internal static class IdentifierPermutations
{
    public static (Reference Identifier, string UrlSegment) ById
        => (Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")), "4b628214-e4fe-4fe0-b1ff-955df33e1515");

    public static (Reference Identifier, string UrlSegment) ByCodename
        => (Reference.ByCodename("codename"), "codename/codename");

    public static (Reference Identifier, string UrlSegment) ByExternalId
        => (Reference.ByExternalId("external-id"), "external-id/external-id");

    public static IEnumerable<(Reference First, string FirstSegment, Reference Second, string SecondSegment)> Pairs(
        IReadOnlyList<(Reference Identifier, string UrlSegment)> first,
        IReadOnlyList<(Reference Identifier, string UrlSegment)> second)
    {
        foreach (var f in first)
        {
            foreach (var s in second)
            {
                yield return (f.Identifier, f.UrlSegment, s.Identifier, s.UrlSegment);
            }
        }
    }
}

/// <summary>
/// ClassData provider yielding every item×language <see cref="LanguageVariantIdentifier"/> permutation
/// paired with the variant endpoint URL it renders to.
/// </summary>
internal sealed class CombinationOfVariantIdentifiersAndUrl : IEnumerable<object[]>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var (identifier, url) in GetPermutation())
        {
            yield return [identifier, url];
        }
    }

    public static IEnumerable<(LanguageVariantIdentifier Identifier, string Url)> GetPermutation()
    {
        var items = new[] { IdentifierPermutations.ById, IdentifierPermutations.ByCodename, IdentifierPermutations.ByExternalId };
        var languages = new[] { IdentifierPermutations.ById, IdentifierPermutations.ByCodename };

        foreach (var (item, itemSegment, language, languageSegment) in IdentifierPermutations.Pairs(items, languages))
        {
            var identifier = new LanguageVariantIdentifier(item, language);
            var url = $"{MockClientFactory.BaseUrl}/items/{itemSegment}/variants/{languageSegment}";
            yield return (identifier, url);
        }
    }
}
