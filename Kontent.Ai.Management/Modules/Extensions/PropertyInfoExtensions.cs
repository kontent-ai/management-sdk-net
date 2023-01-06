using Kontent.Ai.Management.Modules.ModelBuilders;
using Newtonsoft.Json;
using System;
using System.Reflection;


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
            : Guid.Parse(attribute.ElementId);
    }

    /// <summary>
    /// Get Element codename from strongly typed model property.
    /// </summary>
    public static string GetKontentElementCodename(this PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();

        return attribute == null
            ? throw new InvalidOperationException($"Cannot get Kontent.ai element codename as there is no attribute of type {nameof(JsonPropertyAttribute)}")
            : attribute.PropertyName;
    }
}
