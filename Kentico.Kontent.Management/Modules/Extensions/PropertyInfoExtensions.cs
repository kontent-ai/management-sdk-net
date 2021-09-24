using System;
using System.Reflection;
using Kentico.Kontent.Management.Modules.ModelBuilders;

namespace Kentico.Kontent.Management.Modules.Extensions
{
    internal static class PropertyInfoExtensions
    {
        internal static Guid GetKontentElementId(this PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<KontentElementIdAttribute>();

            if(attribute == null)
            {
                throw new InvalidOperationException($"Cannot get kontent element id as there is no attribute of type {nameof(KontentElementIdAttribute)}");
            }

            return Guid.Parse(property.GetCustomAttribute<KontentElementIdAttribute>()?.ElementId);
        }
    }
}