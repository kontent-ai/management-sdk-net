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
        /// Identifier of element
        /// </summary>
        public string ElementId { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ElementIdentifier"/>
        /// </summary>
        /// <param name="itemId">Unique identifier of item</param>
        /// <param name="elementId">Unique identifier of element</param>
        public ElementIdentifier(string itemId, string elementId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentException("Item is not specified.", nameof(itemId));
            }

            if (string.IsNullOrEmpty(elementId))
            {
                throw new ArgumentException("Element is not specified.", nameof(elementId));
            }

            ItemId = itemId;
            ElementId = elementId;
        }
    }
}
