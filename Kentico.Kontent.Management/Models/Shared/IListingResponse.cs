using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models
{
    internal interface IListingResponse<T> : IEnumerable<T>
    {
        PaginationResponseModel Pagination { get; set; }
    }
}
