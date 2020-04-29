﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComicVineApi.Http;

namespace ComicVineApi.Clients
{
    public abstract class ClientBase
    {
        private readonly int endpointId;
        private readonly string filterResource;
        private readonly string getResource;

        protected ClientBase(IApiConnection connection, int endpointId, string filterResource, string getResource)
        {
            this.endpointId = endpointId;
            this.filterResource = filterResource;
            this.getResource = getResource;

            ApiConnection = connection;
        }

        protected IApiConnection ApiConnection { get; }

        protected internal async Task<int> CountAsync<T>()
        {
            var options = new FilterOptions();
            options.FieldList.Add("id");
            options.Limit = 1;
            options.Offset = 0;

            var uri = new Uri(filterResource, UriKind.Relative);
            var result = await ApiConnection.FilterAsync<T>(uri, options).ConfigureAwait(false);
            return result.NumberOfTotalResults;
        }

        protected internal async Task<IReadOnlyList<T>> FilterAsync<T>(FilterOptions? options)
        {
            var uri = new Uri(filterResource, UriKind.Relative);
            var result = await ApiConnection.FilterAsync<T>(uri, options).ConfigureAwait(false);
            return result.Results;
        }

        protected internal async Task<T> GetAsync<T>(int id)
        {
            var uri = new Uri($"{getResource}/{endpointId}-{id}", UriKind.Relative);
            var result = await ApiConnection.GetAsync<T>(uri).ConfigureAwait(false);
            return result.Results;
        }
    }
}
