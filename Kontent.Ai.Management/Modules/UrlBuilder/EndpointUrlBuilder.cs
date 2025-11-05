using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.Extensions;
using Kontent.Ai.Management.Modules.UrlBuilder.Templates;
using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder;

internal sealed class EndpointUrlBuilder
{
    private const int URI_MAX_LENGTH = 65519;

    private const string URL_ASSET_FOLDERS = "/folders";
    private const string URL_TEMPLATE_FILE_FILENAME = "/files/{0}";

    private readonly TaxonomyTemplate _taxonomyTemplate;
    private readonly AssetTemplate _assetTemplate;
    private readonly AssetRenditionTemplate _assetRenditionTemplate;
    private readonly LanguageTemplate _languageTemplate;
    private readonly SnippetTemplate _snippetTemplate;
    private readonly SpaceTemplate _spaceTemplate;
    private readonly CollectionTemplate _collectionTemplate;
    private readonly TypeTemplate _typeTemplate;
    private readonly ValidateTemplate _validateTemplate;
    private readonly VariantTemplate _variantTemplate;
    private readonly WebhookTemplate _webhookTemplate;
    private readonly WorkflowTemplate _workflowTemplate;
    private readonly ItemTemplate _itemTemplate;
    private readonly EnvironmentRolesTemplate _environmentRolesTemplate;
    private readonly UserTemplate _userTemplate;
    private readonly WebSpotlightTemplate _webSpotlightTemplate;
    private readonly VariantFilterTemplate _variantFilterTemplate;
    private readonly CustomAppTemplate _customAppTemplate;

    private readonly ManagementOptions _options;

    public EndpointUrlBuilder(ManagementOptions options)
    {
        _taxonomyTemplate = new TaxonomyTemplate();
        _assetTemplate = new AssetTemplate();
        _assetRenditionTemplate = new AssetRenditionTemplate();
        _languageTemplate = new LanguageTemplate();
        _snippetTemplate = new SnippetTemplate();
        _spaceTemplate = new SpaceTemplate();
        _collectionTemplate = new CollectionTemplate();
        _typeTemplate = new TypeTemplate();
        _validateTemplate = new ValidateTemplate();
        _variantTemplate = new VariantTemplate();
        _webhookTemplate = new WebhookTemplate();
        _workflowTemplate = new WorkflowTemplate();
        _itemTemplate = new ItemTemplate();
        _environmentRolesTemplate = new EnvironmentRolesTemplate();
        _userTemplate = new UserTemplate();
        _webSpotlightTemplate = new WebSpotlightTemplate();
        _variantFilterTemplate = new VariantFilterTemplate();
        _customAppTemplate = new CustomAppTemplate();

        _options = options;
    }

    public string BuildListVariantsByItemUrl(Reference identifier) => GetEnvironmentUrl(string.Concat(_itemTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

    public string BuildListVariantsByTypeUrl(Reference identifier) => GetEnvironmentUrl(string.Concat(_typeTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

    public string BuildListVariantsByComponentUrl(Reference identifier) => GetEnvironmentUrl(string.Concat(_typeTemplate.GetIdentifierUrlSegment(identifier), "/components"));

    public string BuildListVariantsByCollectionUrl(Reference identifier) => GetEnvironmentUrl(string.Concat(_collectionTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

    public string BuildListVariantsBySpaceUrl(Reference identifier) => GetEnvironmentUrl(string.Concat(_spaceTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

    public string BuildVariantsUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
            string.Concat(
                _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier)));

    public string BuildPublishedVariantsUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
        string.Concat(
            _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
            _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
            "/published"));

    public string BuildTypeUrl() => GetEnvironmentUrl(_typeTemplate.Url);

    public string BuildTypeUrl(Reference identifier) => GetEnvironmentUrl(_typeTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildSnippetsUrl() => GetEnvironmentUrl(_snippetTemplate.Url);

    public string BuildSnippetsUrl(Reference identifier) => GetEnvironmentUrl(_snippetTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildSpacesUrl() => GetEnvironmentUrl(_spaceTemplate.Url);

    public string BuildSpacesUrl(Reference identifier) => GetEnvironmentUrl(_spaceTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildTaxonomyUrl() => GetEnvironmentUrl(_taxonomyTemplate.Url);

    public string BuildTaxonomyUrl(Reference identifier) => GetEnvironmentUrl(_taxonomyTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildLanguagesUrl() => GetEnvironmentUrl(_languageTemplate.Url);

    public string BuildLanguagesUrl(Reference identifier) => GetEnvironmentUrl(_languageTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildWebhooksUrl() => GetEnvironmentUrl(_webhookTemplate.Url);

    public string BuildWebhooksUrl(Reference identifier) => GetEnvironmentUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildWebhooksEnableUrl(Reference identifier) =>
        string.Concat(GetEnvironmentUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier)), "/enable");

    public string BuildWebhooksDisableUrl(Reference identifier) =>
        string.Concat(GetEnvironmentUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier)), "/disable");

    public string BuildWorkflowsUrl() => GetEnvironmentUrl(_workflowTemplate.Url);

    public string BuildWorkflowsUrl(Reference identifier) => GetEnvironmentUrl(_workflowTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildWorkflowChangeUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
        string.Concat(
            _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
            _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
            "/change-workflow"));

    public string BuildPublishVariantUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
            string.Concat(
                _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                "/publish"));

    public string BuildSchedulePublishAndUnpublishVariantUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
        string.Concat(
            _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
            _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
            "/schedule-publish-and-unpublish"));

    public string BuildCancelPublishingVariantUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
            string.Concat(
                _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                "/cancel-scheduled-publish"));

    public string BuildUnpublishVariantUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
            string.Concat(
                _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                "/unpublish-and-archive"));

    public string BuildCancelUnpublishingVariantUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
            string.Concat(
                _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                "/cancel-scheduled-unpublish"));
    public string BuildNewVersionVariantUrl(LanguageVariantIdentifier identifier) => GetEnvironmentUrl(
            string.Concat(
                _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                "/new-version"));
    public string BuildItemsUrl() => GetEnvironmentUrl(_itemTemplate.Url);

    public string BuildItemUrl(Reference identifier) => GetEnvironmentUrl(_itemTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildAssetsUrl() => GetEnvironmentUrl(_assetTemplate.Url);

    public string BuildAssetFoldersUrl() => GetEnvironmentUrl(URL_ASSET_FOLDERS);

    public string BuildAssetsUrl(Reference identifier) => GetEnvironmentUrl(_assetTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildUploadFileUrl(string fileName) => GetEnvironmentUrl(string.Format(URL_TEMPLATE_FILE_FILENAME, fileName));

    public string BuildAssetRenditionsUrl(Reference assetIdentifier) => GetEnvironmentUrl(
        string.Concat(
            _assetTemplate.GetIdentifierUrlSegment(assetIdentifier),
            _assetRenditionTemplate.Url));

    public string BuildAssetRenditionsUrl(AssetRenditionIdentifier identifier) => GetEnvironmentUrl(
        string.Concat(
            _assetTemplate.GetIdentifierUrlSegment(identifier.AssetIdentifier),
            _assetRenditionTemplate.GetIdentifierUrlSegment(identifier.RenditionIdentifier)));

    public string BuildEnvironmentRolesUrl() => GetEnvironmentUrl(_environmentRolesTemplate.Url);

    public string BuildEnvironmentRoleUrl(Reference identifier) => GetEnvironmentUrl(_environmentRolesTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildValidationUrl() => GetEnvironmentUrl(_validateTemplate.Url);

    public string BuildAsyncValidationUrl() => GetEnvironmentUrl("/validate-async");

    public string BuildAsyncValidationTaskUrl(Guid taskId) => GetEnvironmentUrl($"/validate-async/tasks/{taskId}");

    public string BuildAsyncValidationTaskIssuesUrl(Guid taskId) => GetEnvironmentUrl($"/validate-async/tasks/{taskId}/issues");

    public string BuildEnvironmentUrl() => string.Format(_options.EndpointV2, $"projects/{_options.EnvironmentId}");

    public string BuildPreviewConfigurationUrl() => GetEnvironmentUrl("/preview-configuration");

    public string BuildCollectionsUrl() => GetEnvironmentUrl(_collectionTemplate.Url);

    public string BuildUsersUrl() => GetEnvironmentUrl(_userTemplate.Url);

    public string BuildModifyUsersRoleUrl(UserIdentifier identifier) => GetEnvironmentUrl(string.Concat(_userTemplate.GetIdentifierUrlSegment(identifier), "/roles"));

    public string BuildSubscriptionProjectsUrl() => GetSubscriptionUrl("/projects");

    public string BuildSubscriptionUsersUrl() => GetSubscriptionUrl(_userTemplate.Url);

    public string BuildSubscriptionUserUrl(UserIdentifier identifier) => GetSubscriptionUrl(_userTemplate.GetIdentifierUrlSegment(identifier));

    public string BuildSubscriptionUserActivateUrl(UserIdentifier identifier) => GetSubscriptionUrl(
        string.Concat(
            _userTemplate.GetIdentifierUrlSegment(identifier),
            "/activate"));

    public string BuildSubscriptionUserDeactivateDisableUrl(UserIdentifier identifier) => GetSubscriptionUrl(
        string.Concat(
            _userTemplate.GetIdentifierUrlSegment(identifier),
            "/deactivate"));

    private string BuildSubscriptionUrl() => string.Format(_options.EndpointV2, $"subscriptions/{_options.SubscriptionId}");

    public string BuildCloneEnvironmentUrl() => GetEnvironmentUrl("/clone-environment");

    public string BuildGetEnvironmentCloningStateUrl() => GetEnvironmentUrl("/environment-cloning-state");

    public string BuildMarkEnvironmentAsProductionUrl() => GetEnvironmentUrl("/mark-environment-as-production");

    public string BuildWebSpotlightUrl() => GetEnvironmentUrl(_webSpotlightTemplate.Url);

    public string BuildCustomAppUrl() => GetEnvironmentUrl(_customAppTemplate.Url);

    public string BuildVariantFilterUrl() => GetEnvironmentUrl(_variantFilterTemplate.Url);

    public string BuildCustomAppUrl(Reference identifier) => GetEnvironmentUrl(string.Concat(_customAppTemplate.GetIdentifierUrlSegment(identifier)));

    private string GetEnvironmentUrl(string path, params string[] parameters) => GetUrl(BuildEnvironmentUrl(), path, parameters);

    private string GetSubscriptionUrl(string path, params string[] parameters) => GetUrl(BuildSubscriptionUrl(), path, parameters);

    private static string GetUrl(string endpointUrl, string path, params string[] parameters)
    {
        var url = string.Concat(endpointUrl, path);

        if (parameters != null && parameters.Length > 0)
        {
            var joinedQuery = string.Join("&", parameters);

            if (!string.IsNullOrEmpty(joinedQuery))
            {
                url = $"{url}?{joinedQuery}";
            }
        }

        return url.Length > URI_MAX_LENGTH
            ? throw new UriFormatException("The request url is too long. Split your query into multiple calls.")
            : url;
    }
}
