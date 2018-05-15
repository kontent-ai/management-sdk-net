using System;
using System.Linq;

using KenticoCloud.ContentManagement.Helpers.Configuration;
using KenticoCloud.ContentManagement.Helpers.Models;

namespace KenticoCloud.ContentManagement.Helpers
{
    /// <summary>
    /// Creates URL for redirection to editing of content items.
    /// </summary>
    public class EditLinkBuilder : IEditLinkBuilder
    {
        private const string URL_TEMPLATE_EDIT_ITEM = "goto/edit-item/project/{0}/variant-codename/{1}/item/{2}";
        private const string URL_TEMPLATE_EDIT_ITEM_ELEMENT = "goto/edit-item/project/{0}/variant-codename/{1}/{2}";
        private const string ITEM_ELEMENT_SEGMENT = "item/{0}/element/{1}";

        private readonly ContentManagementHelpersOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditLinkBuilder"/> class for retrieving edit urls.
        /// </summary>
        /// <param name="contentManagementHelpersOptions">The settings of the Kentico Cloud project.</param>
        public EditLinkBuilder(ContentManagementHelpersOptions contentManagementHelpersOptions)
        {
            if (string.IsNullOrEmpty(contentManagementHelpersOptions.AdminUrl))
            {
                throw new ArgumentException("Kentico Cloud Edit App endpoint is not specified.", nameof(contentManagementHelpersOptions.AdminUrl));
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
        /// <param name="language">Language codename.</param>
        /// <param name="itemId">Identifier of content item.</param>
        /// <returns>URL to edit page of specified item.</returns>
        public string BuildEditItemUrl(string language, string itemId)
        {
            if (string.IsNullOrEmpty(language))
            {
                throw new ArgumentException("Language is not specified.", nameof(language));
            }

            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentException("Item is not specified.", nameof(itemId));
            }

            return string.Format(_options.AdminUrl,
                string.Format(URL_TEMPLATE_EDIT_ITEM, _options.ProjectId, language, itemId));
        }

        /// <summary>
        /// Gets URL to edit page of specified content item.
        /// </summary>
        /// <param name="language">Language codename.</param>
        /// <param name="elementIdentifiers">Identifiers of hierarchy of content item.</param>
        /// <returns>URL to edit page of specified item.</returns>
        public string BuildEditItemUrl(string language, params ElementIdentifier[] elementIdentifiers)
        {
            if (string.IsNullOrEmpty(language))
            {
                throw new ArgumentException("Language is not specified.", nameof(language));
            }

            if (!elementIdentifiers.Any())
            {
                throw new ArgumentException("At least one element identifier must be specified.", nameof(elementIdentifiers));
            }

            var elements = string.Join("/", elementIdentifiers.Select(BuildSingleElementSegment));
            return string.Format(_options.AdminUrl,
                string.Format(URL_TEMPLATE_EDIT_ITEM_ELEMENT, _options.ProjectId, language, elements));
        }

        private string BuildSingleElementSegment(ElementIdentifier elementIdentifier)
        {
            return string.Format(ITEM_ELEMENT_SEGMENT, elementIdentifier.ItemId, elementIdentifier.ElementCodename);
        }
    }
}
