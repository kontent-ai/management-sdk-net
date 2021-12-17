﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Models.Shared
{
    /// <summary>
    /// Represents listing response.
    /// </summary>
    public class ListingResponseModel<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _result;

        private readonly string _continuationToken;
        private readonly string _url;
        private readonly Func<string, string, Task<IListingResponse<T>>> _nextPageRetriever;

        public ListingResponseModel(Func<string, string, Task<IListingResponse<T>>> retriever, string continuationToken, string url, IEnumerable<T> result)
        {
            _nextPageRetriever = retriever;
            _continuationToken = continuationToken;
            _url = url;
            _result = result;
        }

        /// <summary>
        /// Returns strongly typed listing response model.
        /// </summary>
        /// <returns>The <see cref="ListingResponseModel{T}"/> instance that represents the listing provided type.</returns>
        public async Task<ListingResponseModel<T>> GetNextPage()
        {
            if (_continuationToken == null)
            {
                throw new InvalidOperationException("Next page not available.");
            }

            var nextPage = await _nextPageRetriever(_continuationToken, _url);
            return new ListingResponseModel<T>(_nextPageRetriever, nextPage.Pagination?.Token, _url, nextPage);
        }

        /// <summary>
        /// Returns existence of next page.
        /// </summary>
        /// <returns>The <see cref="bool"/>representing existence of the next page.</returns>
        public bool HasNextPage()
        {
            return _continuationToken != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}"/> instance that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _result.GetEnumerator();
        }
    }
}
