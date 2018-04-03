using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public interface IProviderNameSearchProviderQuery
    {
        ISearchResponse<ProviderNameSearchResult> GetResults(string term, int take, PaginationOrientationDetails paginationDetails);
        long GetTotalMatches(string term);
    }
}