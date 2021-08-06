using System.Reflection;
using Kentico.Kontent.Management.Modules.ModelBuilders;

namespace Kentico.Kontent.Management.Modules.Extensions
{
    internal static class PropertyInfoExtensions
    {
        internal static string GetKontentElementId(this PropertyInfo property)
        {
            return property.GetCustomAttribute<KontentElementIdAttribute>()?.ElementId;
        }
    }
}