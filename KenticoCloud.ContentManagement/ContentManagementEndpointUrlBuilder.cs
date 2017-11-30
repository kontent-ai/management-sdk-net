using System;

using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Models.Assets;

namespace KenticoCloud.ContentManagement
{
    internal sealed class EndpointUrlBuilder
    {
        private const int URI_MAX_LENGTH = 65519;

        private const string URL_TEMPLATE_PROJECT = "projects/{0}";

        private const string URL_ITEM = "/items";
        private const string URL_TEMPLATE_ITEM_ID = "/items/{0}";
        private const string URL_TEMPLATE_ITEM_EXTERNAL_ID = "/items/external-id/{0}";
        private const string URL_TEMPLATE_ITEM_CODENAME = "/items/codename/{0}";

        private const string URL_VARIANT = "/variants";
        private const string URL_TEMPLATE_VARIANT_ID = "/variants/{0}";
        private const string URL_TEMPLATE_VARIANT_CODENAME = "/variants/codename/{0}";

        private readonly ContentManagementOptions _options;

        internal EndpointUrlBuilder(ContentManagementOptions options)
        {
            _options = options;
        }

        #region Variants

        internal string BuildListVariantsUrl(ContentItemIdentifier identifier)
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

        #endregion

        #region Items

        internal string BuildItemsUrl()
        {
            return GetUrl(URL_ITEM);
        }

        public string BuildItemsListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl(URL_ITEM, $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl(URL_ITEM);
        }


        internal string BuildItemUrl(ContentItemIdentifier identifier)
        {
            var itemSegment = GetItemUrlSegment(identifier);
            return GetUrl(itemSegment);
        }

        #endregion

        private string GetUrl(string path, params string[] parameters)
        {
            var projectSegment = string.Format(URL_TEMPLATE_PROJECT, _options.ProjectId);

            var endpointUrl = string.Format(_options.Endpoint, projectSegment);
            var url = string.Concat(endpointUrl, path);

            if ((parameters != null) && (parameters.Length > 0))
            {
                var joinedQuery = string.Join("&", parameters);

                if (!String.IsNullOrEmpty(joinedQuery))
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

        private string GetItemUrlSegment(ContentItemIdentifier identifier)
        {

            if (identifier.Id != Guid.Empty)
            {
                return string.Format(URL_TEMPLATE_ITEM_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_ITEM_CODENAME, identifier.Codename);
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return string.Format(URL_TEMPLATE_ITEM_EXTERNAL_ID, identifier.ExternalId);
            }

            throw new ArgumentException("You must provide item's id, codename or externalId");
        }

        private string GetVariantUrlSegment(LanguageIdentifier identifier)
        {

            if (identifier.Id != Guid.Empty)
            {
                return string.Format(URL_TEMPLATE_VARIANT_ID, identifier.Id);
            }

            if (!string.IsNullOrEmpty(identifier.Codename))
            {
                return string.Format(URL_TEMPLATE_VARIANT_CODENAME, identifier.Codename);
            }

            return URL_VARIANT;
        }

        public string BuildAssetListingUrl(string continuationToken = null)
        {
            return (continuationToken != null) ? GetUrl($"/assets", $"continuationToken={Uri.EscapeDataString(continuationToken)}") : GetUrl("/assets");
        }

        public string BuildAssetsUrl()
        {
            return GetUrl($"/assets");
        }

        public string BuildAssetsUrl(AssetIdentifier identifier)
        {
            if (identifier.Id != Guid.Empty)
            {
                return GetUrl($"/assets/{identifier.Id}");
            }

            if (!string.IsNullOrEmpty(identifier.ExternalId))
            {
                return GetUrl($"/assets/external-id/{identifier.ExternalId}");
            }

            throw new ArgumentException("You must provide asset's id, or externalId");
        }

        public string BuildAssetsUrlFromExternalId(string externalId)
        {
            return GetUrl($"/assets/external-id/{externalId}");
        }

        public string BuildUploadFileUrl(string fileName)
        {
            return GetUrl($"/files/{fileName}");
        }
    }
}
