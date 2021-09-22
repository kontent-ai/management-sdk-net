using System;
using System.Reflection;
using Kentico.Kontent.Management.Modules.ModelBuilders;

namespace Kentico.Kontent.Management.Modules.Extensions
{
    internal static class PropertyInfoExtensions
    {
        internal static Guid GetKontentElementId(this PropertyInfo property)
        {
            return Guid.Parse(property.GetCustomAttribute<KontentElementIdAttribute>()?.ElementId);
        }
    }
}