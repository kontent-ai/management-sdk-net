using System.Collections.Generic;
using System.Dynamic;

namespace Kentico.Kontent.Management.Extensions;

/// <summary>
/// Extensions methods for the dynamic type.
/// </summary>
public static class DynamicExtensions
{
    /// <summary>
    /// Extension method that checks whether the source type contains a property
    /// </summary>
    /// <param name="source">Source dynamic object</param>
    /// <param name="propertyName">Property name</param>
    /// <returns></returns>
    public static bool HasProperty(dynamic source, string propertyName)
    {
        if (source is ExpandoObject)
        {
            return ((IDictionary<string, object>)source).ContainsKey(propertyName);
        }

        return source.GetType().GetProperty(propertyName) != null;
    }
}
