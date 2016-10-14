using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class StatsHandler : IRequestHandler<StatsQuery, StatsResponse>
    {
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;

        public StatsHandler(
            IGetStandards getStandards,
            IGetFrameworks getFrameworks)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
        }

        public StatsResponse Handle(StatsQuery message)
        {
            var response = new StatsResponse
            {
                StandardCount = (int)_getStandards.GetStandardsAmount(),
                FrameworkCount = (int)_getFrameworks.GetFrameworksAmount()
            };

            return response;
        }
    }
}