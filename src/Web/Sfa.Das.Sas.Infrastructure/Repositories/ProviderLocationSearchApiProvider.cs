namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ApplicationServices;
    using ApplicationServices.Exceptions;
    using ApplicationServices.Models;
    using Core.Configuration;
    using Core.Domain.Model;

    public sealed class ProviderLocationSearchApiProvider : IProviderLocationSearchProvider
    {
        public SearchResult<StandardProviderSearchResultsItem> SearchStandardProviders(string standardId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter)
        {
            throw new NotImplementedException();
        }

        public SearchResult<FrameworkProviderSearchResultsItem> SearchFrameworkProviders(string frameworkId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}