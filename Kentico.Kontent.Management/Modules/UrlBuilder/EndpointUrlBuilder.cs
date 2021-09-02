using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
using Kentico.Kontent.Management.Modules.UrlBuilder.Templates;
using System;
using System.Net;

namespace Kentico.Kontent.Management.UrlBuilder
{
    internal sealed partial class EndpointUrlBuilder
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

        private readonly ManagementOptions _options;

        internal EndpointUrlBuilder(ManagementOptions options)
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

            _options = options;
        }

        #region Variants

        internal string BuildListVariantsUrl(Reference identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _itemTemplate);

            return GetUrl(string.Concat(itemSegment, _variantTemplate.Url));
        }

        internal string BuildVariantsUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment));
        }

        #endregion

        #region Types
        internal string BuildTypesListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(_typeTemplate.Url, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(_typeTemplate.Url);
        }

        internal string BuildTypeUrl()
        {
            return GetUrl(_typeTemplate.Url);
        }

        internal string BuildTypeUrl(Reference identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _typeTemplate);
            return GetUrl(itemSegment);
        }
        #endregion

        #region TypeSnippets

        internal string BuildSnippetsListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(_snippetTemplate.Url, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(_typeTemplate.Url);
        }

        internal string BuildSnippetsUrl()
        {
            return GetUrl(_snippetTemplate.Url);
        }

        internal string BuildSnippetsUrl(Reference identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _snippetTemplate);
            return GetUrl(itemSegment);
        }

        #endregion

        #region Taxonomies

        internal string BuildTaxonomyListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(_taxonomyTemplate.Url, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(_taxonomyTemplate.Url);
        }

        internal string BuildTaxonomyUrl()
        {
            return GetUrl(_taxonomyTemplate.Url);
        }

        internal string BuildTaxonomyUrl(Reference identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _taxonomyTemplate);
            return GetUrl(itemSegment);
        }

        #endregion

        #region Languages
        internal string BuildLanguagesUrl()
        {
            return GetUrl(_languageTemplate.Url);
        }

        internal string BuildLanguagesUrl(Reference identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _languageTemplate);
            return GetUrl(itemSegment);
        }
        #endregion

        #region Webhooks

        internal string BuildWebhooksUrl()
        {
            return GetUrl(_webhookTemplate.Url);
        }

        internal string BuildWebhooksUrl(ObjectIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _webhookTemplate);
            return GetUrl(itemSegment);
        }

        internal string BuildWebhooksEnableUrl(ObjectIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _webhookTemplate) + "/enable";
            return GetUrl(itemSegment);
        }

        internal string BuildWebhooksDisableUrl(ObjectIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _webhookTemplate) + "/disable";
            return GetUrl(itemSegment);
        }
        #endregion

        #region WorkflowSteps

        internal string BuildWorkflowUrl()
        {
            return GetUrl(_workflowTemplate.Url);
        }

        internal string BuildWorkflowChangeUrl(WorkflowIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);
            var workflowSegment = GetIdentifierUrlSegment(identifier.WorkflowStepIdentifier, _workflowTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment, workflowSegment));
        }

        internal string BuildPublishVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/publish"));
        }

        internal string BuildCancelPublishingVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/cancel-scheduled-publish"));
        }

        internal string BuildUnpublishVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/unpublish-and-archive"));
        }

        internal string BuildCancelUnpublishingVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/cancel-scheduled-unpublish"));
        }

        internal string BuildNewVersionVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier.ItemIdentifier, _itemTemplate);
            var variantSegment = GetIdentifierUrlSegment(identifier.LanguageIdentifier, _variantTemplate);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/new-version"));
        }
        #endregion

        #region Items

        internal string BuildItemsListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(_itemTemplate.Url, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(_itemTemplate.Url);
        }

        internal string BuildItemsUrl()
        {
            return GetUrl(_itemTemplate.Url);
        }

        internal string BuildItemUrl(Reference identifier)
        {
            var itemSegment = GetIdentifierUrlSegment(identifier, _itemTemplate);
            return GetUrl(itemSegment);
        }

        #endregion

        #region Assets

        internal string BuildAssetListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(_assetTemplate.Url, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(_assetTemplate.Url);
        }

        internal string BuildAssetsUrl()
        {
            return GetUrl(_assetTemplate.Url);
        }

        internal string BuildAssetFoldersUrl()
        {
            return GetUrl(URL_ASSET_FOLDERS);
        }

        internal string BuildAssetsUrl(AssetIdentifier identifier)
        {
            var assetSegment = GetIdentifierUrlSegment(identifier, _assetTemplate);
            return GetUrl(assetSegment);
        }

        #endregion

        #region Binary files

        internal string BuildUploadFileUrl(string fileName)
        {
            return GetUrl(string.Format(URL_TEMPLATE_FILE_FILENAME, fileName));
        }

        #endregion

        #region Validation

        internal string BuildValidationUrl()
        {
            return GetUrl(_validateTemplate.Url);
        }

        #endregion

        #region Project

        internal string BuildProjectUrl()
        {
            return string.Format(_options.EndpointV2, $"projects/{_options.ProjectId}");
        }
        
        #endregion

        #region Collections

        internal string BuildCollectionsUrl()
        {
            return GetUrl(_collectionTemplate.Url);
        }
        #endregion

        private string GetIdentifierUrlSegment(Reference identifier, UrlTemplate template)
            => GetIdentifier(template, id: identifier.Id, codename: identifier.Codename, externalId: identifier.ExternalId);


        private string GetIdentifierUrlSegment(NoExternalIdIdentifier identifier, UrlTemplate template)
            => GetIdentifier(template, id: identifier.Id, codename: identifier.Codename);


        private string GetIdentifierUrlSegment(ObjectIdentifier identifier, UrlTemplate template)
            => GetIdentifier(template, id: identifier.Id);

        private string GetIdentifierUrlSegment(AssetIdentifier identifer, UrlTemplate template)
            => GetIdentifier(template, id: identifer.Id, externalId: identifer.ExternalId);

        private string GetIdentifier(UrlTemplate template, Guid? id = null, string codename = null, string externalId = null)
        {
            if (id != null)
            {
                return string.Format(template.UrlId, id);
            }

            if (!string.IsNullOrEmpty(codename))
            {
                return string.Format(template.UrlCodename, codename);
            }

            if (!string.IsNullOrEmpty(externalId))
            {
                var escapedExternalId = WebUtility.UrlEncode(externalId);
                return string.Format(template.UrlExternalId, escapedExternalId);
            }

            throw new ArgumentException("You must provide id, codename or externalId");
        }

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
