using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Shared
{
    /// <summary>
    /// Defines the contract for listing responses
    /// </summary>
    /// <typeparam name="T">Represents paginated response model</typeparam>
    public interface IListingResponse<T> : IEnumerable<T>
    {
        /// <summary>
        /// Represents pagination object
        /// </summary>
        PaginationResponseModel Pagination { get; set; }
    }
}
