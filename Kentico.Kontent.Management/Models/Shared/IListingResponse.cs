using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Shared
{
    public interface IListingResponse<T> : IEnumerable<T>
    {
        PaginationResponseModel Pagination { get; set; }
    }
}
