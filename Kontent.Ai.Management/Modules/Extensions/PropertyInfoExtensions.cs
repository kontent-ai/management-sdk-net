using System;
using System.Reflection;
using Kontent.Ai.Management.Modules.ModelBuilders;

namespace Kontent.Ai.Management.Modules.Extensions;

/// <summary>
/// Extension methods for mapping element to strongly typed model properties.
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// Get Element ID from strongly typed model property.
    /// </summary>
    public static Guid GetKontentElementId(this PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<KontentElementIdAttribute>();

        return attribute == null
            ? throw new InvalidOperationException($"Cannot get Kontent.ai element id as there is no attribute of type {nameof(KontentElementIdAttribute)}")
            : Guid.Parse(property.GetCustomAttribute<KontentElementIdAttribute>()?.ElementId);
    }

    /// <summary>
    /// Get Element External ID from strongly typed model property.
    /// </summary>
    public static string GetKontentElementExternalId(this PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<KontentElementExternalIdAttribute>();

        return attribute == null
            ? throw new InvalidOperationException($"Cannot get Kontent.ai element external id as there is no attribute of type {nameof(KontentElementExternalIdAttribute)}")
            : property.GetCustomAttribute<KontentElementExternalIdAttribute>()?.ElementExternalId;
    }
}
