using System;

using KenticoCloud.ContentManagement.Helpers.Configuration;

namespace KenticoCloud.ContentManagement.Helpers
{
    /// <summary>
    /// Creates URL for redirection to editing of content items.
    /// </summary>
    public class EditLinkBuilder
    {
        private const string URL_EDIT_ITEM_WITH_VARIANT_CODENAME = "goto/edit-item/item/{0}/variant-codename/{1}/project/{2}";

        private readonly ContentManagementHelpersOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditLinkBuilder"/> class for retrieving edit urls.
        /// </summary>
        /// <param name="contentManagementHelpersOptions">The settings of the Kentico Cloud project.</param>
        public EditLinkBuilder(ContentManagementHelpersOptions contentManagementHelpersOptions)
        {
            if (string.IsNullOrEmpty(contentManagementHelpersOptions.EditAppEndpoint))
            {
                throw new ArgumentException("Kentico Cloud Edit App endpoint is not specified.", nameof(contentManagementHelpersOptions.EditAppEndpoint));
            }

            if (string.IsNullOrEmpty(contentManagementHelpersOptions.ProjectId))
            {
                throw new ArgumentException("Kentico Cloud project identifier is not specified.", nameof(contentManagementHelpersOptions.ProjectId));
            }

            if (!Guid.TryParse(contentManagementHelpersOptions.ProjectId, out Guid projectIdGuid))
            {
                throw new ArgumentException($"Provided string is not a valid project identifier ({contentManagementHelpersOptions.ProjectId}).", nameof(contentManagementHelpersOptions.ProjectId));
            }

            _options = contentManagementHelpersOptions;
        }

        /// <summary>
        /// Gets URL to edit page of specified content item.
        /// </summary>
        /// <param name="variantCodename">Codename of variant.</param>
        /// <param name="itemId">Unique identifier of content item.</param>
        /// <returns>URL to edit page of specified item.</returns>
        public string GetEditItemUrl(string variantCodename, Guid itemId)
        {
            if (string.IsNullOrEmpty(variantCodename))
            {
                throw new ArgumentException("Variant codename is not specified.", nameof(variantCodename));
            }

            return string.Format(_options.EditAppEndpoint,
                string.Format(URL_EDIT_ITEM_WITH_VARIANT_CODENAME, itemId, variantCodename, _options.ProjectId));
        }
    }
}
