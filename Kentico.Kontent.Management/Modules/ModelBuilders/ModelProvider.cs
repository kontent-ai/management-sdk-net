using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
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
            //todo rethink
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

                var elementObjectToken = (object)elementObject;
                var propertyInstance = property.PropertyType.GetConstructors()
                    .FirstOrDefault(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(object))
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
                .Where(x => (x.GetMethod?.IsPublic ?? false) && x.PropertyType?.BaseType == typeof(BaseElement) && x.GetValue(variantElements) != null 
                && x.CustomAttributes.Any(att => att.AttributeType == typeof(KontentElementIdAttribute)))
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
