using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Queries;
    using Responses;
    using SFA.DAS.NLog.Logger;

    public sealed class ProviderNameSearchHandler : IAsyncRequestHandler<ProviderNameSearchQuery, ProviderNameSearchResponse>
    {
        private readonly ILog _logger;
        private readonly IProviderNameSearchService _nameSearchService;

        private readonly Dictionary<string, ProviderNameSearchResponseCodes> _searchResponseCodes =
            new Dictionary<string, ProviderNameSearchResponseCodes>
            {
                { "NoSearchResultsFound", ProviderNameSearchResponseCodes.NoSearchResultsFound },
                { "SearchFailed", ProviderNameSearchResponseCodes.SearchFailed },
                { "Success", ProviderNameSearchResponseCodes.Success },
                { "SearchTermTooShort", ProviderNameSearchResponseCodes.SearchTermTooShort },
                { string.Empty, ProviderNameSearchResponseCodes.Success }
            };

        public ProviderNameSearchHandler(
            IProviderNameSearchService nameSearchService, ILog logger)
        {
            _nameSearchService = nameSearchService;
            _logger = logger;
        }

        public async Task<ProviderNameSearchResponse> Handle(ProviderNameSearchQuery message)
        {
            var pageNumber = message.Page <= 0 ? 1 : message.Page;

            var searchResults = await _nameSearchService.SearchProviderNameAndAliases(message.SearchTerm, pageNumber);

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
