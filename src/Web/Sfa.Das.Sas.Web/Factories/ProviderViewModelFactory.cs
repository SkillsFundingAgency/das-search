using System;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
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

        public ApprenticeshipDetailsViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria)
        {
            ApprenticeshipDetailsViewModel courseViewModel = null;

            if (!string.IsNullOrEmpty(criteria.StandardCode))
            {
                var model = _apprenticeshipProviderRepository.GetCourseByStandardCode(
                    criteria.ProviderId,
                    criteria.LocationId,
                    criteria.StandardCode);
                if (model != null)
                {
                    courseViewModel = _mappingService.Map<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>(model);
                    courseViewModel.Training = ApprenticeshipTrainingType.Standard;

                    var apprenticeshipData = _getStandards.GetStandardById(model.Product.Apprenticeship.Code);
                    courseViewModel.ApprenticeshipNameWithLevel = apprenticeshipData.Title;
                    courseViewModel.ApprenticeshipLevel = apprenticeshipData.NotionalEndLevel.ToString();
                }
            }

            if (!string.IsNullOrEmpty(criteria.FrameworkId))
            {
                var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                    criteria.ProviderId,
                    criteria.LocationId,
                    criteria.FrameworkId);

                if (model != null)
                {
                    courseViewModel = _mappingService.Map<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>(model);
                    courseViewModel.Training = ApprenticeshipTrainingType.Framework;
                    var frameworkId = Convert.ToInt32(criteria.FrameworkId);

                    var apprenticeshipData = _getFrameworks.GetFrameworkById(frameworkId);
                    courseViewModel.ApprenticeshipNameWithLevel = apprenticeshipData.Title;
                    courseViewModel.ApprenticeshipLevel = apprenticeshipData.Level.ToString();
                }
            }

            return courseViewModel;
        }
    }
}