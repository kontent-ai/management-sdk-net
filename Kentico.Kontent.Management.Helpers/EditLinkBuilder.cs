using System;
using System.Linq;

using Kentico.Kontent.Management.Helpers.Configuration;
using Kentico.Kontent.Management.Helpers.Models;
using Microsoft.Extensions.Options;

namespace Kentico.Kontent.Management.Helpers
{
    /// <summary>
    /// Creates URL for redirection to editing of content items.
    /// </summary>
    public class EditLinkBuilder : IEditLinkBuilder
    {
        private const string URL_TEMPLATE_EDIT_ITEM = "goto/edit-item/project/{0}/variant-codename/{1}/item/{2}";
        private const string URL_TEMPLATE_EDIT_ITEM_ELEMENT = "goto/edit-item/project/{0}/variant-codename/{1}/{2}";
        private const string ITEM_ELEMENT_SEGMENT = "item/{0}/element/{1}";

        private ManagementHelpersOptions managementHelpersOptions;
        private readonly IOptionsMonitor<ManagementHelpersOptions> managementHelpersOptionsMonitor;

        private ManagementHelpersOptions ManagementHelpersOptions
        {
            get
            {
                return managementHelpersOptionsMonitor != null ? managementHelpersOptionsMonitor.CurrentValue : managementHelpersOptions;
            }
            set
            {
                managementHelpersOptions = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditLinkBuilder"/> class for retrieving edit urls.
        /// </summary>
        /// <param name="managementHelpersOptions">The settings of the Kontent project.</param>
        public EditLinkBuilder(ManagementHelpersOptions managementHelpersOptions)
        {
            if (string.IsNullOrEmpty(managementHelpersOptions.AdminUrl))
            {
                throw new ArgumentException("Kontent Edit App endpoint is not specified.", nameof(managementHelpersOptions.AdminUrl));
            }

            if (string.IsNullOrEmpty(managementHelpersOptions.ProjectId))
            {
                throw new ArgumentException("Kontent project identifier is not specified.", nameof(managementHelpersOptions.ProjectId));
            }

            if (!Guid.TryParse(managementHelpersOptions.ProjectId, out _))
            {
                throw new ArgumentException($"Provided string is not a valid project identifier ({managementHelpersOptions.ProjectId}).", nameof(managementHelpersOptions.ProjectId));
            }

            ManagementHelpersOptions = managementHelpersOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditLinkBuilder"/> class for retrieving edit urls.
        /// </summary>
        /// <param name="managementHelpersOptions">The settings of the Kontent project.</param>
        public EditLinkBuilder(IOptionsMonitor<ManagementHelpersOptions> managementHelpersOptions)
        {
            managementHelpersOptionsMonitor = managementHelpersOptions;
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

            return string.Format(ManagementHelpersOptions.AdminUrl,
                string.Format(URL_TEMPLATE_EDIT_ITEM, ManagementHelpersOptions.ProjectId, language, itemId));
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
            return string.Format(ManagementHelpersOptions.AdminUrl,
                string.Format(URL_TEMPLATE_EDIT_ITEM_ELEMENT, ManagementHelpersOptions.ProjectId, language, elements));
        }

        private string BuildSingleElementSegment(ElementIdentifier elementIdentifier) 
            => string.Format(ITEM_ELEMENT_SEGMENT, elementIdentifier.ItemId, elementIdentifier.ElementCodename);
    }
}
