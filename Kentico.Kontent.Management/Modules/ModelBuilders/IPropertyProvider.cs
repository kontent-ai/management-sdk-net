using System;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    /// <summary>
    ///
    /// </summary>
    public interface IPropertyProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetPropertyNameById(Type type, string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetPropertyIdByName(Type type, string name);
    }
}
