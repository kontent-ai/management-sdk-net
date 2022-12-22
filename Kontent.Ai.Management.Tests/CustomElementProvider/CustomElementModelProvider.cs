using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.Extensions;
using Kontent.Ai.Management.Modules.ModelBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kontent.Ai.Management.Tests.CustomElementProvider;

internal class CustomElementModelProvider : IElementModelProvider
{
    private readonly IManagementClient _managementClient;

    internal CustomElementModelProvider(IManagementClient managementClient)
    {
        _managementClient = managementClient;
    }

    public T GetStronglyTypedElements<T>(IEnumerable<dynamic> elements) where T : new()
    {
        var type = typeof(T);

        var model = _managementClient.GetContentTypeAsync(Reference.ByCodename(GetCodename(type.Name))).Result;

        var instance = new T();

        var properties = type.GetProperties().Where(x => x.SetMethod?.IsPublic ?? false).ToList();

        foreach (var element in elements)
        {
            var property = properties.FirstOrDefault(x =>
                x.PropertyType.BaseType == typeof(BaseElement) &&
                model.Elements.Any(c =>
                    c.Id == Guid.Parse(element.element.id) &&
                    c.ExternalId == x.GetCustomAttribute<KontentElementExternalIdAttribute>().ElementExternalId));

            if (property == null)
            {
                continue;
            }

            property.SetValue(instance, BaseElement.FromDynamic(element, property.PropertyType));
        }

        return instance;
    }

    public IEnumerable<dynamic> GetDynamicElements<T>(T stronglyTypedElements)
    {
        var type = typeof(T);

        var elements = type.GetProperties()
            .Where(x => (x.GetMethod?.IsPublic ?? false) && x.PropertyType?.BaseType == typeof(BaseElement) && x.GetValue(stronglyTypedElements) != null)
            .Select(x =>
            {
                var element = (BaseElement)x.GetValue(stronglyTypedElements);
                element.Element = Reference.ByExternalId(x.GetKontentElementExternalId());
                return element?.ToDynamic();
            });

        return elements;
    }

    private static string GetCodename(string name)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < name.Length; i++)
        {
            if (i == 0)
            {
                sb.Append(char.ToLower(name[0]));
            }
            else if (char.IsUpper(name[i]))
            {
                sb.Append($"_{char.ToLower(name[i])}");
            }
            else
            {
                sb.Append(name[i]);
            }
        }

        return sb.ToString();
    }
}
