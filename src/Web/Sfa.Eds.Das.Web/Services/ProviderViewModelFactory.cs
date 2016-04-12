namespace Sfa.Das.Web.Services
{
    using System;

    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public class ProviderViewModelFactory : IProviderViewModelFactory
    {
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        private readonly IGetFrameworks _getFrameworks;

        private readonly IGetStandards _getStandards;

        private readonly IMappingService _mappingService;

        public ProviderViewModelFactory(
            IMappingService mappingService,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks)
        {
            _mappingService = mappingService;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
        }

        public ProviderViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria)
        {
            ProviderViewModel viewModel = null;

            if (!string.IsNullOrEmpty(criteria.StandardCode))
            {
                var model = _apprenticeshipProviderRepository.GetByStandardCode(
                    criteria.ProviderId,
                    criteria.LocationId,
                    criteria.StandardCode);
                if (model != null)
                {
                    viewModel = _mappingService.Map<Provider, ProviderViewModel>(model);
                    viewModel.Training = TrainingEnum.Standard;

                    var apprenticeshipData = _getStandards.GetStandardById(model.Apprenticeship.Code);
                    viewModel.ApprenticeshipNameWithLevel = string.Concat(
                        apprenticeshipData.Title,
                        " level ",
                        apprenticeshipData.NotionalEndLevel);
                }
            }

            if (!string.IsNullOrEmpty(criteria.FrameworkId))
            {
                var model = _apprenticeshipProviderRepository.GetByFrameworkId(
                    criteria.ProviderId,
                    criteria.LocationId,
                    criteria.FrameworkId);

                if (model != null)
                {
                    viewModel = _mappingService.Map<Provider, ProviderViewModel>(model);
                    viewModel.Training = TrainingEnum.Framework;
                    var frameworkId = Convert.ToInt32(criteria.FrameworkId);

                    var apprenticeshipData = _getFrameworks.GetFrameworkById(frameworkId);
                    viewModel.ApprenticeshipNameWithLevel =
                        $"{apprenticeshipData.FrameworkName} - {apprenticeshipData.PathwayName} level {apprenticeshipData.Level}";
                }
            }

            return viewModel;
        }
    }
}