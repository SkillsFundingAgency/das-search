using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    using System.Linq;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using ViewModels;

    public static class ProviderDetailViewModelMapper
    {
        public static ProviderDetailViewModel GetProviderDetailViewModel(Provider provider, ApprenticeshipTrainingSummary apprenticeshipTrainingSummary, IEnumerable<long> hideAboutThisProviderForUlns = null)
        {
            hideAboutThisProviderForUlns = hideAboutThisProviderForUlns ?? new List<long>();

            var employerSatisfationMessage =
                (provider.EmployerSatisfaction > 0)
                    ? ProviderMappingHelper.GetPercentageText(provider.EmployerSatisfaction)
                    : ProviderMappingHelper.GetPercentageText(null);

            var learnerSatisfationMessage =
                (provider.LearnerSatisfaction > 0)
                    ? ProviderMappingHelper.GetPercentageText(provider.LearnerSatisfaction)
                    : ProviderMappingHelper.GetPercentageText(null);

            var viewModel = new ProviderDetailViewModel {
                DisplayAboutThisProvider = !hideAboutThisProviderForUlns.Contains(provider.Ukprn),
                Email = provider.Email,
	            CurrentlyNotStartingNewApprentices = provider.CurrentlyNotStartingNewApprentices,
				IsEmployerProvider = provider.IsEmployerProvider,
                EmployerSatisfaction = provider.EmployerSatisfaction,
                EmployerSatisfactionMessage = employerSatisfationMessage,
                IsHigherEducationInstitute = provider.IsHigherEducationInstitute,
                LearnerSatisfaction = provider.LearnerSatisfaction,
                LearnerSatisfactionMessage = learnerSatisfationMessage,
                NationalProvider = provider.NationalProvider,
                Phone = provider.Phone,
                UkPrn = provider.Ukprn,
                ProviderName = provider.ProviderName,
                Website = provider.Website,
                MarketingInfo = provider.MarketingInfo,
                ApprenticeshipTrainingSummary = apprenticeshipTrainingSummary,
                HasParentCompanyGuarantee = provider.HasParentCompanyGuarantee,
                IsNew = provider.IsNew,
                IsLevyPayerOnly = provider.IsLevyPayerOnly
                };

            if (provider.Aliases != null && provider.Aliases.Any())
                {
                viewModel.TradingNames = provider.Aliases.Aggregate((aggregatingTradingNames, aliasToAdd) => aggregatingTradingNames + ", " +  aliasToAdd);

                }

            return viewModel;
        }
    }
}