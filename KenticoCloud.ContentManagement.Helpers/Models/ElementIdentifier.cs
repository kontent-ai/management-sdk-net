using System;

namespace KenticoCloud.ContentManagement.Helpers.Models
{
    /// <summary>
    /// Identifier of element of some item
    /// </summary>
    public class ElementIdentifier
    {
        /// <summary>
        /// Identifier of item
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Codename of element
        /// </summary>
        public string ElementCodename { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ElementIdentifier"/>
        /// </summary>
        /// <param name="itemId">Unique identifier of item</param>
        /// <param name="elementCodename">Codename of element</param>
        public ElementIdentifier(string itemId, string elementCodename)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentException("Item is not specified.", nameof(itemId));
            }

            if (string.IsNullOrEmpty(elementCodename))
            {
                throw new ArgumentException("Element is not specified.", nameof(elementCodename));
            }

            ItemId = itemId;
            ElementCodename = elementCodename;
        }
    }
}
