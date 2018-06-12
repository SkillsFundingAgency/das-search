namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Queries;
    using Responses;

    public sealed class ProviderNameSearchHandler : IAsyncRequestHandler<ProviderNameSearchQuery, ProviderNameSearchResponse>
    {
        private readonly IProviderNameSearchService _nameSearchService;

        public ProviderNameSearchHandler(
            IProviderNameSearchService nameSearchService)
        {
            _nameSearchService = nameSearchService;
        }

        public async Task<ProviderNameSearchResponse> Handle(ProviderNameSearchQuery message)
        {
            var searchResults = await _nameSearchService.SearchProviderNameAndAliases(message.SearchTerm, message.Page);

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
