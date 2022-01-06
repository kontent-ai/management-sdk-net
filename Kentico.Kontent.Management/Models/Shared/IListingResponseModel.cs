using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Models.Shared
{
    /// <summary>
    /// Represents listing response.
    /// </summary>
    public interface IListingResponseModel<T> : IEnumerable<T>
    {
        /// <summary>
        /// Returns strongly typed listing response model.
        /// </summary>
        /// <returns>The <see cref="IListingResponseModel{T}"/> instance that represents the listing provided type.</returns>
        Task<IListingResponseModel<T>> GetNextPage();

        /// <summary>
        /// Returns existence of next page.
        /// </summary>
        /// <returns>The <see cref="bool"/>representing existence of the next page.</returns>
        bool HasNextPage();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}"/> instance that can be used to iterate through the collection.</returns>
        new IEnumerator<T> GetEnumerator();
    }
}
