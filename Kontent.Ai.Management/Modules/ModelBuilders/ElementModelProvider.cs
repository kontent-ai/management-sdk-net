using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kontent.Ai.Management.Modules.ModelBuilders;

/// <summary>
/// Provides methods for converting between dynamic and strongly-typed models.
/// </summary>
public class ElementModelProvider : IElementModelProvider
{
    /// <inheritdoc />
    public T GetStronglyTypedElements<T>(IEnumerable<dynamic> elements) where T : new()
    {
        var type = typeof(T);
        var instance = new T();

        var properties = type.GetProperties().Where(x => x.SetMethod?.IsPublic ?? false).ToList();

        foreach (var element in elements)
        {
            var property = properties.FirstOrDefault(x => x.PropertyType?.BaseType == typeof(BaseElement) && x.GetCustomAttribute<KontentElementIdAttribute>().ElementId == element.element.id);
            if (property == null)
            {
                continue;
            }

            property.SetValue(instance, BaseElement.FromDynamic(element, property.PropertyType));
        }

        return instance;
    }

    /// <inheritdoc />
    public IEnumerable<dynamic> GetDynamicElements<T>(T stronglyTypedElements)
    {
        var type = typeof(T);

        var elements = type.GetProperties()
            .Where(x => (x.GetMethod?.IsPublic ?? false) && x.PropertyType?.BaseType == typeof(BaseElement) && x.GetValue(stronglyTypedElements) != null)
            .Select(x =>
            {
                var element = (BaseElement)x.GetValue(stronglyTypedElements);
                element.Element = Reference.ById(x.GetKontentElementId());
                return element?.ToDynamic();
            });

        return elements;
    }
}
