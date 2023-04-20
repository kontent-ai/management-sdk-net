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
}
