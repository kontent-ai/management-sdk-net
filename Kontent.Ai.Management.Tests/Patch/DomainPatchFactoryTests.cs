using AwesomeAssertions;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Languages.Patch;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

namespace Kontent.Ai.Management.Tests.Patch;

// The Language / Space / TaxonomyGroup / CustomApp patch factories: each must pin the property enum, the verb, and
// carry the correctly-typed value that the raw models only document in prose.
public class DomainPatchFactoryTests
{
    [Fact]
    public void LanguagePatch_CoversEveryProperty()
    {
        LanguagePatch.Name("Deutsch").Should().BeEquivalentTo(
            new { PropertyName = LanguagePropertyName.Name, Value = "Deutsch" });
        LanguagePatch.Codename("de-DE").Should().BeEquivalentTo(
            new { PropertyName = LanguagePropertyName.Codename, Value = "de-DE" });
        LanguagePatch.FallbackLanguage(Reference.ByCodename("en-US")).Should().BeEquivalentTo(
            new { PropertyName = LanguagePropertyName.FallbackLanguage, Value = Reference.ByCodename("en-US") });
        LanguagePatch.IsActive(false).Should().BeEquivalentTo(
            new { PropertyName = LanguagePropertyName.IsActive, Value = false });
    }

    [Fact]
    public void SpacePatch_CoversEveryProperty()
    {
        SpacePatch.Name("Marketing site").Should().BeEquivalentTo(
            new { PropertyName = SpacePropertyName.Name, Value = "Marketing site" });
        SpacePatch.Codename("marketing").Should().BeEquivalentTo(
            new { PropertyName = SpacePropertyName.Codename, Value = "marketing" });
        SpacePatch.RootItem(Reference.ByCodename("home")).Should().BeEquivalentTo(
            new { PropertyName = SpacePropertyName.RootItem, Value = Reference.ByCodename("home") });
        SpacePatch.RootItem(null).Value.Should().BeNull();
        SpacePatch.Collections(Reference.ByCodename("web")).Should().BeEquivalentTo(
            new { PropertyName = SpacePropertyName.Collections, Value = new[] { Reference.ByCodename("web") } });
    }

    [Fact]
    public void TaxonomyGroupPatch_TargetsGroupOrTerm()
    {
        var term = Reference.ByCodename("coffee");

        TaxonomyGroupPatch.ReplaceName(term, "Coffee beans").Should().BeEquivalentTo(
            new { Reference = term, PropertyName = TaxonomyGroupPropertyName.Name, Value = "Coffee beans" });
        TaxonomyGroupPatch.ReplaceCodename(term, "coffee_beans").Should().BeEquivalentTo(
            new { Reference = term, PropertyName = TaxonomyGroupPropertyName.Codename, Value = "coffee_beans" });

        var terms = TaxonomyGroupPatch.ReplaceTerms(term, new TaxonomyTermCreateModel { Name = "Arabica" });
        terms.PropertyName.Should().Be(TaxonomyGroupPropertyName.Terms);
        terms.Value.Should().BeEquivalentTo(new[] { new TaxonomyTermCreateModel { Name = "Arabica" } });
    }

    [Fact]
    public void CustomAppPatch_PinsVerbPerOperation()
    {
        var role = Reference.ById(Guid.NewGuid());

        CustomAppPatch.ReplaceName("My app").Op.Should().Be("replace");
        CustomAppPatch.ReplaceSourceUrl("https://example.org").Should().BeEquivalentTo(
            new { PropertyName = CustomAppPropertyName.SourceUrl, Value = "https://example.org" });
        CustomAppPatch.ReplaceConfig(null).Value.Should().BeNull();
        CustomAppPatch.ReplaceDisplayMode(CustomAppDisplayMode.Dialog).Should().BeEquivalentTo(
            new { PropertyName = CustomAppPropertyName.DisplayMode, Value = CustomAppDisplayMode.Dialog });
        CustomAppPatch.ReplaceAllowedRoles(role).Value.Should().BeEquivalentTo(new[] { role });
        CustomAppPatch.AddAllowedRole(role).Should().BeEquivalentTo(
            new { PropertyName = CustomAppPropertyName.AllowedRoles, Value = role });
        CustomAppPatch.AddAllowedRole(role).Op.Should().Be("addInto");
        CustomAppPatch.RemoveAllowedRole(role).Op.Should().Be("remove");
    }
}
