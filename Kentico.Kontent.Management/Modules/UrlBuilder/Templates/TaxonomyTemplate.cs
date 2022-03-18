namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates;

internal class TaxonomyTemplate : UrlTemplate
{
    public override string Url => "/taxonomies";
    public override string UrlId => "/taxonomies/{0}";
    public override string UrlCodename => "/taxonomies/codename/{0}";
    public override string UrlExternalId => "/taxonomies/external-id/{0}";
}
