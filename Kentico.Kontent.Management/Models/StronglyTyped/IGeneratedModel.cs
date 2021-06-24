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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetPropertyIdByName(string name);
    }
}
