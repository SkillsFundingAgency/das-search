using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class StatsHandler : IRequestHandler<StatsQuery, StatsResponse>
    {
        private readonly IApprenticeshipSearchService _searchService;

        public StatsHandler(IApprenticeshipSearchService searchService)
        {
            _searchService = searchService;
        }

        public StatsResponse Handle(StatsQuery message)
        {
            var response = new StatsResponse();

            return response;
        }
    }
}