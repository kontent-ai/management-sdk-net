using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
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

            var type = typeof(T);
            var instance = new T();

            var properties = type.GetProperties().Where(x => x.SetMethod?.IsPublic ?? false).ToList();

            foreach (var elementObject in variant.Elements)
            {
                // TODO fix element.element
                var property = properties.FirstOrDefault(x => x.GetCustomAttribute<KontentElementIdAttribute>(true).ElementId == elementObject.element.id);
                if (property == null)
                {
                    continue;
                }

                var value = GetTypedElementValue(property.PropertyType, elementObject);
                if (value != null)
                {
                    property.SetValue(instance, value);
                }
            }

            result.Elements = instance;
            return result;
        }

        public ContentItemVariantUpsertModel GetContentItemVariantUpsertModel<T>(T variantElements) where T : new()
        {
            var type = typeof(T);

            var elements = type.GetProperties()
                .Where(x => (x.GetMethod?.IsPublic ?? false) && x.GetValue(variantElements) != null)
                .Select(x =>
                {
                    // TODO add strongly typed elements
                    if (x.PropertyType == typeof(UrlSlug))
                    {
                        var elementId = x.GetKontentElementId();
                        var slug = (dynamic)x.GetValue(variantElements);

                        return new
                        {
                            element = new { id = elementId },
                            value = slug.Value,
                            mode = slug.Mode
                        };
                    }

                    return (dynamic)new
                    {
                        element = new { id = x.GetKontentElementId() },
                        value = x.GetValue(variantElements)
                    };
                });

            var result = new ContentItemVariantUpsertModel
            {
                Elements = elements
            };

            return result;
        }

        private static object GetTypedElementValue(Type propertyType, dynamic element)
        {
            if (element.value == null)
            {
                return null;
            }

            if (propertyType == typeof(UrlSlug))
            {
                return new UrlSlug
                {
                    Mode = element.mode.ToString(),
                    Value = element.value.ToString()
                };
            }

            if (element.value.GetType() == propertyType)
            {
                return element.value;
            }

            if (propertyType == typeof(string))
            {
                return element.value.ToString();
            }

            if (IsNumericType(propertyType))
            {
                return Convert.ChangeType(element.value, propertyType);
            }

            if (IsNullableType(propertyType) && IsNumericType(Nullable.GetUnderlyingType(propertyType)))
            {
                return Convert.ChangeType(element.value, Nullable.GetUnderlyingType(propertyType));
            }

            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(element.value);
            }

            if (IsArrayType(propertyType))
            {
                return JArray.FromObject(element.value)?.ToObject(propertyType);
            }

            return element.value;
        }

        private static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                default:
                    return false;
            }
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsArrayType(Type type)
        {
            var supportedGenericTypes = new List<Type> { typeof(AssetIdentifier), typeof(ContentItemIdentifier), typeof(MultipleChoiceOptionIdentifier), typeof(TaxonomyTermIdentifier) };

            var isGeneric = type.GetTypeInfo().IsGenericType;
            var isCollection = isGeneric && type.GetInterfaces().Any(gt =>
                    gt.GetTypeInfo().IsGenericType
                    && gt.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICollection<>))
                    && type.GetTypeInfo().IsClass;
            var isEnumerable = isGeneric && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            var hasSupportedGenericArgument = type.GenericTypeArguments.Length == 1 && supportedGenericTypes.Contains(type.GenericTypeArguments[0]);

            if ((isCollection || isEnumerable) && hasSupportedGenericArgument)
            {
                return true;
            }

            if (type.IsArray && supportedGenericTypes.Contains(type.GetElementType()))
            {
                return true;

            }

            return false;
        }
    }
}
