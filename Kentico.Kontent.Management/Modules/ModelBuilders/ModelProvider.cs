using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Items.Elements;
using Kentico.Kontent.Management.Models.StronglyTyped;
using Kentico.Kontent.Management.Modules.Extensions;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    internal class ModelProvider : IModelProvider
    {
        internal ModelProvider() { }

        public ContentItemVariantModel<T> GetContentItemVariantModel<T>(ContentItemVariantModel variant) where T : new()
        {
            var result = new ContentItemVariantModel<T>
            {
                Item = variant.Item,
                Language = variant.Language,
                LastModified = variant.LastModified
            };

            // TODO validate switching reflection to custom JSON serializer
            var type = typeof(T);
            var instance = new T();

            var properties = type.GetProperties().Where(x => x.SetMethod?.IsPublic ?? false).ToList();

            foreach (var elementObject in variant.Elements)
            {
                var property = properties.FirstOrDefault(x => x.PropertyType?.BaseType == typeof(BaseElement) && x.GetCustomAttribute<KontentElementIdAttribute>().ElementId == elementObject.element.id);
                if (property == null)
                {
                    continue;
                }

                var elementObjectToken = (JToken)elementObject;
                var propertyInstance = property.PropertyType.GetConstructors()
                    .FirstOrDefault(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(JToken))
                    .Invoke(new object[] { elementObjectToken });

                property.SetValue(instance, propertyInstance);
            }

            result.Elements = instance;
            return result;
        }

        public ContentItemVariantUpsertModel GetContentItemVariantUpsertModel<T>(T variantElements) where T : new()
        {
            var type = typeof(T);

            var elements = type.GetProperties()
                .Where(x => (x.GetMethod?.IsPublic ?? false) && x.PropertyType?.BaseType == typeof(BaseElement) && x.GetValue(variantElements) != null)
                .Select(x =>
                {
                    var element = (BaseElement)x.GetValue(variantElements);
                    return element?.ToDynamic(x.GetKontentElementId());
                });

            var result = new ContentItemVariantUpsertModel
            {
                Elements = elements
            };

            return result;
        }
    }
}
