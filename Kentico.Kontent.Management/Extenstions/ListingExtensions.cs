using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Extenstions
{
    /// <summary>
    /// Extensions methods related to listing endpoints
    /// </summary>
    public static class ListingExtensions
    {
        /// <summary>
        /// This is extension method for listing methods from <see cref="IManagementClient"/>. 
        /// It goes page by page until it gets all items for given entity.
        /// </summary>
        /// <typeparam name="T">Entity model e.g <see cref="LanguageModel"/>, <see cref="TaxonomyGroupModel"/> and etc.</typeparam>
        /// <param name="method">To be extended method that returns <see cref="Task{IListingResponseModel}"/></param>
        /// <returns><see cref="List{T}"/> all items for project</returns>
        public static async Task<List<T>> GetAllAsync<T>(this Task<IListingResponseModel<T>> method)
        {
            var result = new List<T>();
            var response = await method;

            while (true)
            {
                result.AddRange(response);

                if (!response.HasNextPage())
                {
                    break;
                }

                response = await response.GetNextPage();
            }

            return result;
        }
    }
}
