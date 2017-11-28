using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models.Shared
{
    public interface IListingResponse<T> : IEnumerable<T>
    {
        PaginationResponseModel Pagination { get; set; }
    }
}
