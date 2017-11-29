using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models
{
    public interface IListingResponse<T> : IEnumerable<T>
    {
        PaginationResponseModel Pagination { get; set; }
    }
}
