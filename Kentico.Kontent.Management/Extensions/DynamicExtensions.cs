using System.Collections.Generic;
using System.Dynamic;

namespace Kentico.Kontent.Management.Extensions
{
    public static class DynamicExtensions
    {
        public static bool HasProperty(dynamic source, string propertyName)
        {
            if (source is ExpandoObject)
            {
                return ((IDictionary<string, object>)source).ContainsKey(propertyName);
            }

            return source.GetType().GetProperty(propertyName) != null;
        }
    }
}
