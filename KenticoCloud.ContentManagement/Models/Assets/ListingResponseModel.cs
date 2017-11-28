using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Models.Shared;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public class ListingResponseModel<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _elements;

        private readonly string _continuationToken;

        private readonly Func<string, Task<IListingResponse<T>>> _nextPageRetriever;

        internal ListingResponseModel(Func<string, Task<IListingResponse<T>>> retriever, string continuationToken, IEnumerable<T> elements)
        {
            _nextPageRetriever = retriever;
            _continuationToken = continuationToken;
            _elements = elements;
        }

        public async Task<ListingResponseModel<T>> GetNextPage()
        {
            if (_continuationToken == null)
            {
                throw new InvalidOperationException("Next page not available.");
            }
            var nextPage = await _nextPageRetriever(_continuationToken);
            return new ListingResponseModel<T>(_nextPageRetriever, nextPage.Pagination?.Token, nextPage);
        }

        public bool HasNextPage()
        {
            return _continuationToken != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
