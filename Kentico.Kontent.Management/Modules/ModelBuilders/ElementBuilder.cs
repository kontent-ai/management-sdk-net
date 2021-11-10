using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    public static class ElementBuilder
    {
        public static IEnumerable<dynamic> GetElementsAsDynamic(params BaseElement[] elements)
        {
            return elements.Select(element => element.ToDynamic());
        }
    }
}