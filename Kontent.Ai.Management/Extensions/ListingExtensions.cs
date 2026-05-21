using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontent.Ai.Management.Extensions;

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
    /// <returns><see cref="List{T}"/> all items for environment</returns>
    public async static Task<List<T>> GetAllAsync<T>(this Task<IListingResponseModel<T>> method)
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

    /// <summary>
    /// This is extension method for result-returning listing methods from <see cref="IManagementClient"/>.
    /// It goes page by page until it gets all items for given entity.
    /// </summary>
    /// <typeparam name="T">Entity model e.g <see cref="LanguageModel"/>, <see cref="TaxonomyGroupModel"/> and etc.</typeparam>
    /// <param name="method">A listing method whose result wraps an <see cref="IListingResponseModel{T}"/>.</param>
    /// <returns>A result wrapping every item across all pages on success, or the listing failure detail.</returns>
    public async static Task<IManagementResult<IReadOnlyList<T>>> GetAllAsync<T>(this Task<IManagementResult<IListingResponseModel<T>>> method)
    {
        var result = await method;
        if (!result.IsSuccess)
        {
            return ManagementResult<IReadOnlyList<T>>.Failure(result.Error!, result.StatusCode, result.RequestUrl, result.ResponseHeaders);
        }

        var all = new List<T>();
        var page = result.Value;

        while (true)
        {
            all.AddRange(page);

            if (!page.HasNextPage())
            {
                break;
            }

            page = await page.GetNextPage();
        }

        return ManagementResult<IReadOnlyList<T>>.Success(all, result.StatusCode, result.RequestUrl, result.ResponseHeaders);
    }
}
