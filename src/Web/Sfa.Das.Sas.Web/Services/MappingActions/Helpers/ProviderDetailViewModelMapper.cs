namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    using System.Linq;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using ViewModels;

    public static class ProviderDetailViewModelMapper
    {
        public static ProviderDetailViewModel GetProviderDetailViewModel(Provider provider, ApprenticeshipTrainingSummary apprenticeshipTrainingSummary)
        {
            var viewModel = new ProviderDetailViewModel {HasMoreThanOneTradingName = false };

            if (provider.Aliases != null && provider.Aliases.Any())
            {
                viewModel.TradingNames = provider.Aliases.Aggregate((aggregatingTradingNames, aliasToAdd) => aggregatingTradingNames + ", " + aliasToAdd);
                if (provider.Aliases.Count() > 1)
                {
                    viewModel.HasMoreThanOneTradingName = true;
                }
            }

            var employerSatisfationMessage =
                (provider.EmployerSatisfaction > 0)
                    ? ProviderMappingHelper.GetPercentageText(provider.EmployerSatisfaction)
                    : ProviderMappingHelper.GetPercentageText(null);

            var learnerSatisfationMessage =
                (provider.LearnerSatisfaction > 0)
                    ? ProviderMappingHelper.GetPercentageText(provider.LearnerSatisfaction)
                    : ProviderMappingHelper.GetPercentageText(null);

            viewModel.Email = provider.Email;
            viewModel.IsEmployerProvider = provider.IsEmployerProvider;
            viewModel.EmployerSatisfaction = provider.EmployerSatisfaction;
            viewModel.EmployerSatisfactionMessage = employerSatisfationMessage;
            viewModel.IsHigherEducationInstitute = provider.IsHigherEducationInstitute;
            viewModel.LearnerSatisfaction = provider.LearnerSatisfaction;
            viewModel.LearnerSatisfactionMessage = learnerSatisfationMessage;
            viewModel.NationalProvider = provider.NationalProvider;
            viewModel.Phone = provider.Phone;
            viewModel.UkPrn = provider.Ukprn;
            viewModel.ProviderName = provider.ProviderName;
            viewModel.Website = provider.Website;
            viewModel.MarketingInfo = provider.MarketingInfo;
            viewModel.ApprenticeshipTrainingSummary = apprenticeshipTrainingSummary;
            viewModel.HasParentCompanyGuarantee = provider.HasParentCompanyGuarantee;
            viewModel.IsNew = provider.IsNew;
            viewModel.IsLevyPayerOnly = provider.IsLevyPayerOnly;
            return viewModel;
        }
    }
}