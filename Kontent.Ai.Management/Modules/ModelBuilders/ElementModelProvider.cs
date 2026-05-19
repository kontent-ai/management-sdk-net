using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.Extensions;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

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

        foreach (var raw in elements)
        {
            // Post-cutover the elements are JsonElement; the legacy BaseElement.FromDynamic path
            // expects a dynamic with `.element.id`-style accessors. Adapt at this boundary — the
            // BaseElement hierarchy itself stays a documented holdout until its phase-7+ removal.
            var element = raw is JsonElement je ? JsonElementToExpando(je) : raw;
            string elementId = element.element.id;
            var property = properties.FirstOrDefault(x => x.PropertyType?.BaseType == typeof(BaseElement) && x.GetCustomAttribute<KontentElementIdAttribute>().ElementId == elementId);
            if (property == null)
            {
                continue;
            }

            property.SetValue(instance, BaseElement.FromDynamic(element, property.PropertyType));
        }

        return instance;
    }

    private static dynamic JsonElementToExpando(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                IDictionary<string, object> obj = new ExpandoObject();
                foreach (var prop in element.EnumerateObject())
                {
                    obj[prop.Name] = JsonElementToExpando(prop.Value);
                }
                return obj;
            case JsonValueKind.Array:
                return element.EnumerateArray().Select(e => (object)JsonElementToExpando(e)).ToList();
            case JsonValueKind.String:
                return element.GetString()!;
            case JsonValueKind.Number:
                return element.TryGetDecimal(out var dec) ? dec : (object)element.GetDouble();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            default:
                return null!;
        }
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
