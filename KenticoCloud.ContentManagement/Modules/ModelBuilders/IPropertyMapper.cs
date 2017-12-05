using System;
using System.Collections.Generic;
using System.Reflection;

namespace KenticoCloud.ContentManagement.Modules.ModelBuilders
{
    /// <summary>
    /// Defines the contract for mapping Kentico Cloud content item fields to model properties.
    /// </summary>
    public interface IPropertyMapper
    {
        /// <summary>
        /// Determines whether the given property corresponds to a given field.
        /// </summary>
        /// <param name="modelProperty">CLR property to be compared with <paramref name="elementName"/>.</param>
        /// <param name="elementName">Codename of the field in Kentico Cloud content type.</param>
        /// <returns>TRUE if <paramref name="modelProperty"/> is a CLR representation of <paramref name="elementName"/> in Kentico Cloud content type.</returns>
        bool IsMatch(PropertyInfo modelProperty, string elementName);

        /// <summary>
        /// Returns the mapping between model property name and codename of field in Kentico Cloud content type
        /// </summary>
        /// <param name="type">Type of the model.</param>
        /// <returns>Dictionary where key is CLR property name and value is a field codename.</returns>
        IDictionary<string, string> GetNameMapping(Type type);
    }
}