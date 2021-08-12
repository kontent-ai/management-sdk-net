using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
using System;
using System.Net;

namespace Kentico.Kontent.Management
{
    internal sealed class EndpointUrlBuilder
    {
        private const int URI_MAX_LENGTH = 65519;

        private const string URL_ITEM = "/items";
        private const string URL_TEMPLATE_ITEM_ID = "/items/{0}";
        private const string URL_TEMPLATE_ITEM_EXTERNAL_ID = "/items/external-id/{0}";
        private const string URL_TEMPLATE_ITEM_CODENAME = "/items/codename/{0}";

        private const string URL_VARIANT = "/variants";
        private const string URL_TEMPLATE_VARIANT_ID = "/variants/{0}";
        private const string URL_TEMPLATE_VARIANT_CODENAME = "/variants/codename/{0}";

        private const string URL_ASSET = "/assets";
        private const string URL_TEMPLATE_ASSET_ID = "/assets/{0}";
        private const string URL_TEMPLATE_ASSET_EXTERNAL_ID = "/assets/external-id/{0}";

        private const string URL_ASSET_FOLDERS = "/folders";

        private const string URL_TEMPLATE_FILE_FILENAME = "/files/{0}";

        private const string URL_VALIDATE = "/validate";

        private const string URL_TYPES = "/types";
        private const string URL_TEMPLATE_TYPES_ID = "/types/{0}";
        private const string URL_TEMPLATE_TYPES_CODENAME = "/types/codename/{0}";
        private const string URL_TEMPLATE_TYPES_EXTERNAL_ID = "/types/external-id/{0}";

        private const string URL_TAXONOMY = "/taxonomies";
        private const string URL_TEMPLATE_TAXONOMY_ID = "/taxonomies/{0}";
        private const string URL_TEMPLATE_TAXONOMY_CODENAME = "/taxonomies/codename/{0}";
        private const string URL_TEMPLATE_TAXONOMY_EXTERNAL_ID = "/taxonomies/external-id/{0}";

        private const string URL_LANGUAGE = "/languages";
        private const string URL_TEMPLATE_LANGUAGE_ID = "/languages/{0}";
        private const string URL_TEMPLATE_LANGUAGE_CODENAME = "/languages/codename/{0}";
        private const string URL_TEMPLATE_LANGUAGE_EXTERNAL_ID = "/languages/external-id/{0}";

        private const string URL_WEBHOOKS = "/webhooks";
        private const string URL_TEMPLATE_WEBHOOKS_ID = "/webhooks/{0}";

        private const string URL_WORKFLOW = "/workflow";
        private const string URL_TEMPLATE_WORKFLOW_ID = "/workflow/{0}";
        private const string URL_TEMPLATE_WORKFLOW_CODENAME = "/workflow/codename/{0}";


        private readonly ManagementOptions _options;

        internal EndpointUrlBuilder(ManagementOptions options)
        {
            _options = options;
        }

        #region Variants

        internal string BuildListVariantsUrl(Reference identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier);

            return GetUrl(string.Concat(itemSegment, URL_VARIANT));
        }

        internal string BuildVariantsUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment));
        }

        private string GetVariantUrlSegment(Reference identifier)
        {
            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_VARIANT_CODENAME, identifier.Codename);
            }

            return string.Format(URL_TEMPLATE_VARIANT_ID, identifier.Id);
        }

        #endregion

        #region Types
        internal string BuildTypesListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(URL_TYPES, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(URL_TYPES);
        }

        internal string BuildListTypesUrl()
        {
            return GetUrl(string.Concat(URL_TYPES));
        }

        internal string BuildTypeUrl()
        {
            return GetUrl(URL_TYPES);
        }

        internal string BuildTypeUrl(Reference identifier)
        {
            var itemSegment = GetTypeUrlSegment(identifier);
            return GetUrl(itemSegment);
        }

        private string GetTypeUrlSegment(Reference identifier)
        {
            if (identifier.Id != null)
            {
                return string.Format(URL_TEMPLATE_TYPES_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_TYPES_CODENAME, identifier.Codename);
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return BuildTypeUrlSegmentFromExternalId(identifier.ExternalId);
            }

            throw new ArgumentException("You must provide item's id, codename or externalId");
        }

        internal string BuildTypeUrlSegmentFromExternalId(string externalId)
        {
            var escapedExternalId = WebUtility.UrlEncode(externalId);
            return string.Format(URL_TEMPLATE_TYPES_EXTERNAL_ID, escapedExternalId);
        }

        #endregion

        #region Taxonomies

        internal string BuildTaxonomyListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(URL_TAXONOMY, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(URL_TYPES);
        }

        internal string BuildTaxonomyUrl()
        {
            return GetUrl(URL_TAXONOMY);
        }

        internal string BuildTaxonomyUrl(Reference identifier)
        {
            var itemSegment = GetTaxonomyUrlSegment(identifier);
            return GetUrl(itemSegment);
        }

        //todo this method is good candidate for refactoring as its here many times (types, items)
        private string GetTaxonomyUrlSegment(Reference identifier)
        {
            if (identifier.Id != null)
            {
                return string.Format(URL_TEMPLATE_TAXONOMY_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_TAXONOMY_CODENAME, identifier.Codename);
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return BuildTaxonomiesUrlSegmentFromExternalId(identifier.ExternalId);
            }

            throw new ArgumentException("You must provide item's id, codename or externalId");
        }

        //todo this method is good candidate for refactoring as its here many times (types, items)
        internal string BuildTaxonomiesUrlSegmentFromExternalId(string externalId)
        {
            var escapedExternalId = WebUtility.UrlEncode(externalId);
            return string.Format(URL_TEMPLATE_TAXONOMY_EXTERNAL_ID, escapedExternalId);
        }

        #endregion

        #region Languages
        internal string BuildLanguagesUrl()
        {
            return GetUrl(URL_LANGUAGE);
        }

        internal string BuildLanguagesUrl(Reference identifier)
        {
            var itemSegment = GetLanguagesUrlSegment(identifier);
            return GetUrl(itemSegment);
        }

        //todo this method is good candidate for refactoring as its here many times (types, items)
        private string GetLanguagesUrlSegment(Reference identifier)
        {
            if (identifier.Id != null)
            {
                return string.Format(URL_TEMPLATE_LANGUAGE_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_LANGUAGE_CODENAME, identifier.Codename);
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return BuildLanguagesUrlSegmentFromExternalId(identifier.ExternalId);
            }

            throw new ArgumentException("You must provide language's id, codename or externalId");
        }

        //todo this method is good candidate for refactoring as its here many times (types, items)
        internal string BuildLanguagesUrlSegmentFromExternalId(string externalId)
        {
            var escapedExternalId = WebUtility.UrlEncode(externalId);
            return string.Format(URL_TEMPLATE_LANGUAGE_EXTERNAL_ID, escapedExternalId);
        }
        #endregion

        #region Webhooks

        internal string BuildWebhooksUrl()
        {
            return GetUrl(URL_WEBHOOKS);
        }

        internal string BuildWebhooksUrl(ObjectIdentifier identifier)
        {
            var itemSegment = GetWebhooksUrlSegment(identifier);
            return GetUrl(itemSegment);
        }

        internal string BuildWebhooksEnableUrl(ObjectIdentifier identifier)
        {
            var itemSegment = GetWebhooksUrlSegment(identifier) + "/enable";
            return GetUrl(itemSegment);
        }

        internal string BuildWebhooksDisableUrl(ObjectIdentifier identifier)
        {
            var itemSegment = GetWebhooksUrlSegment(identifier) + "/disable";
            return GetUrl(itemSegment);
        }

        //todo this method is good candidate for refactoring as its here many times (types, items)
        private string GetWebhooksUrlSegment(ObjectIdentifier identifier)
        {
            if (identifier.Id != null)
            {
                return string.Format(URL_TEMPLATE_WEBHOOKS_ID, identifier.Id);
            }

            throw new ArgumentException("You must provide webhook's id");
        }
        #endregion

        #region WorkflowSteps

        internal string BuildWorkflowUrl()
        {
            return GetUrl(URL_WORKFLOW);
        }

        internal string BuildWorkflowChangeUrl(WorkflowIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);
            var workflowSegment = GetWorkflowUrlSegment(identifier.WorkflowStepIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment, workflowSegment));
        }

        internal string BuildPublishVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/publish"));
        }

        internal string BuildCancelPublishingVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/cancel-scheduled-publish"));
        }

        internal string BuildUnpublishVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/unpublish-and-archive"));
        }

        internal string BuildCancelUnpublishingVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/cancel-scheduled-unpublish"));
        }

        internal string BuildNewVersionVariantUrl(ContentItemVariantIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier.ItemIdentifier);
            var variantSegment = GetVariantUrlSegment(identifier.LanguageIdentifier);

            return GetUrl(string.Concat(itemSegment, variantSegment, "/new-version"));
        }

        private string GetWorkflowUrlSegment(NoExternalIdIdentifier identifier)
        {
            if (identifier.Id != null)
            {
                return string.Format(URL_TEMPLATE_WORKFLOW_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_WORKFLOW_CODENAME, identifier.Codename);
            }

            throw new ArgumentException("You must provide language's id, codename or externalId");
        }
        #endregion

        #region Items

        internal string BuildItemsListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(URL_ITEM, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(URL_ITEM);
        }

        internal string BuildItemsUrl()
        {
            return GetUrl(URL_ITEM);
        }

        internal string BuildItemUrl(Reference identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier);
            return GetUrl(itemSegment);
        }

        private string GetItemUrlSegment(Reference identifier)
        {
            if (identifier.Id != null)
            {
                return string.Format(URL_TEMPLATE_ITEM_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_ITEM_CODENAME, identifier.Codename);
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return BuildItemUrlSegmentFromExternalId(identifier.ExternalId);
            }
            throw new ArgumentException("You must provide item's id, codename or externalId");
        }

        internal string BuildItemUrlSegmentFromExternalId(string externalId)
        {
            var escapedExternalId = WebUtility.UrlEncode(externalId);
            return string.Format(URL_TEMPLATE_ITEM_EXTERNAL_ID, escapedExternalId);
        }

        #endregion

        #region Assets

        public string BuildAssetListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(URL_ASSET, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(URL_ASSET);
        }

        public string BuildAssetsUrl()
        {
            return GetUrl(URL_ASSET);
        }

        public string BuildAssetFoldersUrl()
        {
            return GetUrl(URL_ASSET_FOLDERS);
        }

        public string BuildAssetsUrl(AssetIdentifier identifier)
        {
            if (identifier.Id != null)
            {
                return GetUrl(string.Format(URL_TEMPLATE_ASSET_ID, identifier.Id));
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return BuildAssetsUrlFromExternalId(identifier.ExternalId);
            }

            throw new ArgumentException("You must provide asset's id, or externalId");
        }

        public string BuildAssetsUrlFromExternalId(string externalId)
        {
            var escapedExternalId = WebUtility.UrlEncode(externalId);
            return GetUrl(string.Format(URL_TEMPLATE_ASSET_EXTERNAL_ID, escapedExternalId));
        }

        #endregion

        #region Binary files

        public string BuildUploadFileUrl(string fileName)
        {
            return GetUrl(string.Format(URL_TEMPLATE_FILE_FILENAME, fileName));
        }

        #endregion

        #region Validation

        public string BuildValidationUrl()
        {
            return GetUrl(URL_VALIDATE);
        }

        #endregion

        private string GetUrl(string path, params string[] parameters)
        {
            var projectSegment = $"projects/{_options.ProjectId}";

            var endpointUrl = string.Format(_options.EndpointV2, projectSegment);
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
