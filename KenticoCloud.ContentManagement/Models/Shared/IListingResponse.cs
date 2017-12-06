using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models
{
    /// <summary>
    /// Represents interface for listing response.
    /// </summary>
    public interface IListingResponse<T> : IEnumerable<T>
    {
        PaginationResponseModel Pagination { get; set; }
    }
}
