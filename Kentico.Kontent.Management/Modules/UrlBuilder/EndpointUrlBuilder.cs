using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.UrlBuilder.Templates;
using System;
using Kentico.Kontent.Management.Models.AssetRenditions;
using Kentico.Kontent.Management.Configuration;
using Kentico.Kontent.Management.Modules.Extensions;

namespace Kentico.Kontent.Management.Modules.UrlBuilder
{
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
        private readonly CollectionTemplate _collectionTemplate;
        private readonly TypeTemplate _typeTemplate;
        private readonly ValidateTemplate _validateTemplate;
        private readonly VariantTemplate _variantTemplate;
        private readonly WebhookTemplate _webhookTemplate;
        private readonly WorkflowTemplate _workflowTemplate;
        private readonly ItemTemplate _itemTemplate;
        private readonly ProjectRolesTemplate _projectRolesTemplate;
        private readonly UserTemplate _userTemplate;

        private readonly ManagementOptions _options;

        public EndpointUrlBuilder(ManagementOptions options)
        {
            _taxonomyTemplate = new TaxonomyTemplate();
            _assetTemplate = new AssetTemplate();
            _assetRenditionTemplate = new AssetRenditionTemplate();
            _languageTemplate = new LanguageTemplate();
            _snippetTemplate = new SnippetTemplate();
            _collectionTemplate = new CollectionTemplate();
            _typeTemplate = new TypeTemplate();
            _validateTemplate = new ValidateTemplate();
            _variantTemplate = new VariantTemplate();
            _webhookTemplate = new WebhookTemplate();
            _workflowTemplate = new WorkflowTemplate();
            _itemTemplate = new ItemTemplate();
            _projectRolesTemplate = new ProjectRolesTemplate();
            _userTemplate = new UserTemplate();

            _options = options;
        }

        public string BuildListVariantsByItemUrl(Reference identifier) => GetProjectUrl(string.Concat(_itemTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

        public string BuildListVariantsByTypeUrl(Reference identifier) => GetProjectUrl(string.Concat(_typeTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

        public string BuildListVariantsByComponentUrl(Reference identifier) => GetProjectUrl(string.Concat(_typeTemplate.GetIdentifierUrlSegment(identifier), "/components"));

        public string BuildListVariantsByCollectionUrl(Reference identifier) => GetProjectUrl(string.Concat(_collectionTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

        public string BuildVariantsUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier)));

        public string BuildTypeUrl() => GetProjectUrl(_typeTemplate.Url);

        public string BuildTypeUrl(Reference identifier) => GetProjectUrl(_typeTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildSnippetsUrl() => GetProjectUrl(_snippetTemplate.Url);

        public string BuildSnippetsUrl(Reference identifier) => GetProjectUrl(_snippetTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildTaxonomyUrl() => GetProjectUrl(_taxonomyTemplate.Url);

        public string BuildTaxonomyUrl(Reference identifier) => GetProjectUrl(_taxonomyTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildLanguagesUrl() => GetProjectUrl(_languageTemplate.Url);

        public string BuildLanguagesUrl(Reference identifier) => GetProjectUrl(_languageTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWebhooksUrl() => GetProjectUrl(_webhookTemplate.Url);

        public string BuildWebhooksUrl(Reference identifier) => GetProjectUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWebhooksEnableUrl(Reference identifier) => GetProjectUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWebhooksDisableUrl(Reference identifier) => GetProjectUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));
        
        public string BuildWorkflowsUrl() => GetProjectUrl(_workflowTemplate.Url);

        public string BuildWorkflowsUrl(Reference identifier) => GetProjectUrl(_workflowTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWorkflowChangeUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/change-workflow"));

        public string BuildPublishVariantUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/publish"));

        public string BuildCancelPublishingVariantUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/cancel-scheduled-publish"));

        public string BuildUnpublishVariantUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/unpublish-and-archive"));

        public string BuildCancelUnpublishingVariantUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/cancel-scheduled-unpublish"));
        public string BuildNewVersionVariantUrl(LanguageVariantIdentifier identifier) => GetProjectUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/new-version"));
        public string BuildItemsUrl() => GetProjectUrl(_itemTemplate.Url);

        public string BuildItemUrl(Reference identifier) => GetProjectUrl(_itemTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildAssetsUrl() => GetProjectUrl(_assetTemplate.Url);

        public string BuildAssetFoldersUrl() => GetProjectUrl(URL_ASSET_FOLDERS);

        public string BuildAssetsUrl(Reference identifier) => GetProjectUrl(_assetTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildUploadFileUrl(string fileName) => GetProjectUrl(string.Format(URL_TEMPLATE_FILE_FILENAME, fileName));

        public string BuildAssetRenditionsUrl(Reference assetIdentifier) => GetProjectUrl(
            string.Concat(
                _assetTemplate.GetIdentifierUrlSegment(assetIdentifier),
                _assetRenditionTemplate.Url));

        public string BuildAssetRenditionsUrl(AssetRenditionIdentifier identifier) => GetProjectUrl(
            string.Concat(
                _assetTemplate.GetIdentifierUrlSegment(identifier.AssetIdentifier),
                _assetRenditionTemplate.GetIdentifierUrlSegment(identifier.RenditionIdentifier)));

        public string BuildProjectRolesUrl() => GetProjectUrl(_projectRolesTemplate.Url);

        public string BuildProjectRoleUrl(Reference identifier) => GetProjectUrl(_projectRolesTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildValidationUrl() => GetProjectUrl(_validateTemplate.Url);

        public string BuildProjectUrl() => string.Format(_options.EndpointV2, $"projects/{_options.ProjectId}");

        public string BuildCollectionsUrl() => GetProjectUrl(_collectionTemplate.Url);

        public string BuildUsersUrl() => GetProjectUrl(_userTemplate.Url);

        public string BuildModifyUsersRoleUrl(UserIdentifier identifier) => GetProjectUrl(string.Concat(_userTemplate.GetIdentifierUrlSegment(identifier), "/roles"));

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

        public string BuildCloneEnvironmentUrl() => GetProjectUrl("/clone-environment");

        public string BuildGetEnvironmentCloningStateUrl() => GetProjectUrl("/environment-cloning-state");

        public string BuildMarkEnvironmentAsProductionUrl() => GetProjectUrl("/mark-environment-as-production");

        private string GetProjectUrl(string path, params string[] parameters) => GetUrl(BuildProjectUrl(), path, parameters);

        private string GetSubscriptionUrl(string path, params string[] parameters) => GetUrl(BuildSubscriptionUrl(), path, parameters);

        private string GetUrl(string endpointUrl, string path, params string[] parameters)
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

            if (url.Length > URI_MAX_LENGTH)
            {
                throw new UriFormatException("The request url is too long. Split your query into multiple calls.");
            }

            return url;
        }
    }
}
