using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Shared;

internal interface IListingResponse<T> : IEnumerable<T>
{
    PaginationResponseModel Pagination { get; set; }
}
