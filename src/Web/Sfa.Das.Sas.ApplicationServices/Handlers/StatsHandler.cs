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
        private readonly IGetProviders _getProviders;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        public StatsHandler(
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            IGetProviders getProviders,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _getProviders = getProviders;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
        }

        public StatsResponse Handle(StatsQuery message)
        {
            var response = new StatsResponse
            {
                StandardCount = (int)_getStandards.GetStandardsAmount(),
                FrameworkCount = (int)_getFrameworks.GetFrameworksAmount(),
                ProviderCount = (int)_getProviders.GetProvidersAmount(),
                ExpiringFrameworks = _getFrameworks.GetFrameworksExpiringSoon(),
                StandardsWithProviders = _apprenticeshipProviderRepository.GetStandardsAmountWithProviders(),
                FrameworksWithProviders = _apprenticeshipProviderRepository.GetFrameworksAmountWithProviders()
            };

            response.StandardsWithoutProviders = response.StandardCount - response.StandardsWithProviders;
            response.FrameworksWithoutProviders = response.FrameworkCount - response.FrameworksWithProviders;

            return response;
        }
    }
}