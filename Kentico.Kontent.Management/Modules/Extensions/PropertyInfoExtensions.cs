using System.Reflection;
using Kentico.Kontent.Management.Modules.ModelBuilders;

namespace Kentico.Kontent.Management.Modules.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetKontentElementId(this PropertyInfo property)
        {
            return property.GetCustomAttribute<KontentElementIdAttribute>()?.ElementId;
        }
    }
}