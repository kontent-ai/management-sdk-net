using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using System;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    /// <summary>
    /// Provides extension methods for building dynamic elements from strongly typed element based on the <see cref="BaseElement"/>.
    /// </summary>
    public static class ElementBuilder
    {
        /// <summary>
        /// Builds a dynamic elements enumeration from a strongly typed elements based on the <see cref="BaseElement"/>.
        /// </summary>
        public static IEnumerable<dynamic> GetElementsAsDynamic(params BaseElement[] elements)
        {
            elements.Where(elementObject => elementObject.Element == null).ToList().ForEach(element =>
            {
                throw new ArgumentNullException("Element identifier (`BaseElement.Element` property) not set for element on index ", Array.IndexOf(elements, element).ToString());
            });

            elements.Where(element => !element.Element.DoesHaveSetOnlyOneIdentifier()).ToList().ForEach(element =>
            {
                throw new ArgumentException("Element must have only one identifier set (`BaseElement.Element` property).", nameof(element));
            });

            return elements.Select(element => element.ToDynamic());
        }
    }
}