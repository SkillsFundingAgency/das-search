using System;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services
{
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
                    viewModel.Training = ApprenticeshipTrainingType.Standard;

                    var apprenticeshipData = _getStandards.GetStandardById(model.Apprenticeship.Code);
                    viewModel.ApprenticeshipNameWithLevel = apprenticeshipData.Title;
                    viewModel.ApprenticeshipLevel = apprenticeshipData.NotionalEndLevel.ToString();
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
                    viewModel.Training = ApprenticeshipTrainingType.Framework;
                    var frameworkId = Convert.ToInt32(criteria.FrameworkId);

                    var apprenticeshipData = _getFrameworks.GetFrameworkById(frameworkId);
                    viewModel.ApprenticeshipNameWithLevel = apprenticeshipData.Title;
                    viewModel.ApprenticeshipLevel = apprenticeshipData.Level.ToString();
                }
            }

            return viewModel;
        }
    }
}