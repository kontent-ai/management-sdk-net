using System;
using System.Collections.Generic;
using System.Text;

namespace Kentico.Kontent.Management.Models.StronglyTyped
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGeneratedModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetPropertyNameById(string id);

        string GetPropertyIdByName(string name);
    }
}
