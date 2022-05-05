using Kentico.Kontent.Management.Models.TaxonomyGroups.Patch;
using Kentico.Kontent.Management.Models.Types.Patch;
using System;
using System.Collections;
using System.Linq;

namespace Kentico.Kontent.Management.Tests.Base.Converters;
internal static class ConverterHelper
{
    public static bool IsCollectionOf<T>(Type objectType) =>
        objectType.IsGenericType &&
        typeof(IEnumerable).IsAssignableFrom(objectType.GetGenericTypeDefinition()) &&
        objectType.GenericTypeArguments.Length == 1 &&
        objectType.GenericTypeArguments.FirstOrDefault() == typeof(T);

    public static bool IsOperationModel<T>() =>
        typeof(T) == typeof(TaxonomyGroupOperationBaseModel) ||
        typeof(T) == typeof(ContentTypeOperationBaseModel);

    public static bool HasProperty<T>(string propertyName) => typeof(T).GetProperty(propertyName) != null;

    public static object GetPropertyValue(this object obj, string propName) => obj.GetType().GetProperty(propName).GetValue(obj, null);
}
