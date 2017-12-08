using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models
{
    internal interface IListingResponse<T> : IEnumerable<T>
    {
        PaginationResponseModel Pagination { get; set; }
    }
}
