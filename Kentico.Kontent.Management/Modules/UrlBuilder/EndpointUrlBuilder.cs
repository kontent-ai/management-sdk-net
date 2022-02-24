using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
using Kentico.Kontent.Management.Modules.UrlBuilder;
using Kentico.Kontent.Management.Modules.UrlBuilder.Templates;
using System;

namespace Kentico.Kontent.Management.UrlBuilder
{
    internal sealed class EndpointUrlBuilder
    {
        private const int URI_MAX_LENGTH = 65519;

        private const string URL_ASSET_FOLDERS = "/folders";
        private const string URL_TEMPLATE_FILE_FILENAME = "/files/{0}";

        private readonly TaxonomyTemplate _taxonomyTemplate;
        private readonly AssetTemplate _assetTemplate;
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

        public string BuildListVariantsByItemUrl(Reference identifier) => GetUrl(string.Concat(_itemTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

        public string BuildListVariantsByTypeUrl(Reference identifier) => GetUrl(string.Concat(_typeTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

        public string BuildListVariantsByComponentUrl(Reference identifier) => GetUrl(string.Concat(_typeTemplate.GetIdentifierUrlSegment(identifier), "/components"));

        public string BuildListVariantsByCollectionUrl(Reference identifier) => GetUrl(string.Concat(_collectionTemplate.GetIdentifierUrlSegment(identifier), _variantTemplate.Url));

        public string BuildVariantsUrl(LanguageVariantIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier)));

        public string BuildTypeUrl() => GetUrl(_typeTemplate.Url);

        public string BuildTypeUrl(Reference identifier) => GetUrl(_typeTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildSnippetsUrl() => GetUrl(_snippetTemplate.Url);

        public string BuildSnippetsUrl(Reference identifier) => GetUrl(_snippetTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildTaxonomyUrl() => GetUrl(_taxonomyTemplate.Url);

        public string BuildTaxonomyUrl(Reference identifier) => GetUrl(_taxonomyTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildLanguagesUrl() => GetUrl(_languageTemplate.Url);

        public string BuildLanguagesUrl(Reference identifier) => GetUrl(_languageTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWebhooksUrl() => GetUrl(_webhookTemplate.Url);

        public string BuildWebhooksUrl(Reference identifier) => GetUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWebhooksEnableUrl(Reference identifier) => GetUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWebhooksDisableUrl(Reference identifier) => GetUrl(_webhookTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildWorkflowUrl() => GetUrl(_workflowTemplate.Url);

        public string BuildWorkflowChangeUrl(WorkflowIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    _workflowTemplate.GetIdentifierUrlSegment(identifier.WorkflowStepIdentifier)));

        public string BuildPublishVariantUrl(LanguageVariantIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/publish"));

        public string BuildCancelPublishingVariantUrl(LanguageVariantIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/cancel-scheduled-publish"));

        public string BuildUnpublishVariantUrl(LanguageVariantIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/unpublish-and-archive"));

        public string BuildCancelUnpublishingVariantUrl(LanguageVariantIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/cancel-scheduled-unpublish"));
        public string BuildNewVersionVariantUrl(LanguageVariantIdentifier identifier) => GetUrl(
                string.Concat(
                    _itemTemplate.GetIdentifierUrlSegment(identifier.ItemIdentifier),
                    _variantTemplate.GetIdentifierUrlSegment(identifier.LanguageIdentifier),
                    "/new-version"));
        public string BuildItemsUrl() => GetUrl(_itemTemplate.Url);

        public string BuildItemUrl(Reference identifier) => GetUrl(_itemTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildAssetsUrl() => GetUrl(_assetTemplate.Url);

        public string BuildAssetFoldersUrl() => GetUrl(URL_ASSET_FOLDERS);

        public string BuildAssetsUrl(Reference identifier) => GetUrl(_assetTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildUploadFileUrl(string fileName) => GetUrl(string.Format(URL_TEMPLATE_FILE_FILENAME, fileName));

        internal string BuildProjectRolesUrl() => GetUrl(_projectRolesTemplate.Url);

        internal string BuildProjectRoleUrl(Reference identifier) => GetUrl(_projectRolesTemplate.GetIdentifierUrlSegment(identifier));

        public string BuildValidationUrl() => GetUrl(_validateTemplate.Url);

        public string BuildProjectUrl() => string.Format(_options.EndpointV2, $"projects/{_options.ProjectId}");

        public string BuildCollectionsUrl() => GetUrl(_collectionTemplate.Url);

        public string BuildUsersUrl() => GetUrl(_userTemplate.Url);

        public string BuildModifyUsersRoleUrl(UserIdentifier identifier) => GetUrl(string.Concat(_userTemplate.GetIdentifierUrlSegment(identifier), "/roles"));

        public string BuildCloneEnvironmentUrl() => GetUrl("/clone-environment");

        public string BuildGetEnvironmentCloningStateUrl() => GetUrl("/environment-cloning-state");

        public string BuildMarkEnvironmentAsProductionUrl() => GetUrl("/mark-environment-as-production");

        private string GetUrl(string path, params string[] parameters)
        {
            var endpointUrl = BuildProjectUrl();
            var url = string.Concat(endpointUrl, path);

            if ((parameters != null) && (parameters.Length > 0))
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
