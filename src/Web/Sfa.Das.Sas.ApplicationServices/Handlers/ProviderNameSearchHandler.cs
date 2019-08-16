using System.Threading;
using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Queries;
    using Responses;

    public sealed class ProviderNameSearchHandler : IRequestHandler<ProviderNameSearchQuery, ProviderNameSearchResponse>
    {
        private readonly IProviderSearchProvider _searchProviderName;
        private readonly PaginationSettings _paginationSettings;

        public ProviderNameSearchHandler(
            IProviderSearchProvider searchProviderName, IOptions<PaginationSettings> paginationSettings)
        {
            _searchProviderName = searchProviderName;
            _paginationSettings = paginationSettings.Value;
        }

        public async Task<ProviderNameSearchResponse> Handle(ProviderNameSearchQuery message, CancellationToken cancellationToken)
        {

            if (message.PageSize == 0)
            {
                message.PageSize = _paginationSettings.DefaultResultsAmount;
            }

            if (message.Page == 0)
            {
                message.Page = 1;
            }

            var searchResults = await _searchProviderName.SearchProviderNameAndAliases(message.SearchTerm, message.Page, message.PageSize);

            var providerNameSearchResponse = new ProviderNameSearchResponse
            {
                ActualPage = searchResults.ActualPage,
                HasError = searchResults.HasError,
                LastPage = searchResults.LastPage,
                Results = searchResults.Results,
                TotalResults = searchResults.TotalResults,
                ResultsToTake = searchResults.ResultsToTake,
                SearchTerm = message.SearchTerm,
                StatusCode = searchResults.ResponseCode
            };

            return providerNameSearchResponse;
        }
    }
}
