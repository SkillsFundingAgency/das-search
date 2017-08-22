using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

namespace Sfa.Das.Sas.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.DAS.Providers.Api.Client;
    using Sfa.Das.Sas.Web.ViewModels;

    public class ProviderService : IProviderService
    {

        private readonly IProviderApiClient _providerApiClient;

        public ProviderService(IProviderApiClient providerApiClient)
        {
            _providerApiClient = providerApiClient;
        }

        public Dictionary<long, string> GetProviderList()
        {
            var res = _providerApiClient.FindAll()
                .ToDictionary(x => x.Ukprn, x => x.ProviderName);

            return res;
        }

        public ProviderDetailsViewModel GetProviderDetails(long prn)
        {
            var provider = _providerApiClient.Get(prn);
            var viewModel = new ProviderDetailsViewModel();
            if (provider.Aliases != null)
            {
                viewModel.TradingNames = provider.Aliases.ToList().Aggregate((i, j) => i + ", " + j);
            }

            var employerSatisfationMessage =
                (provider.EmployerSatisfaction > 0)
                ? ProviderMappingHelper.GetPercentageText(provider.EmployerSatisfaction) :
                ProviderMappingHelper.GetPercentageText(null);

            var learnerSatisfationMessage =
                (provider.LearnerSatisfaction > 0)
                    ? ProviderMappingHelper.GetPercentageText(provider.LearnerSatisfaction) :
                    ProviderMappingHelper.GetPercentageText(null);

            viewModel.Email = provider.Email;
            viewModel.IsEmployerProvider = provider.IsEmployerProvider;
            viewModel.EmployerSatisfaction = provider.EmployerSatisfaction;
            viewModel.EmployerSatisfactionMessage = employerSatisfationMessage;
            viewModel.IsHigherEducationInstitute = provider.IsHigherEducationInstitute;
            viewModel.LearnerSatisfaction = provider.LearnerSatisfaction;
            viewModel.LearnerSatisfactionMessage = learnerSatisfationMessage;
            viewModel.NationalProvider = provider.NationalProvider;
            viewModel.Phone = provider.Phone;
            viewModel.Ukprn = provider.Ukprn;
            viewModel.ProviderName = provider.ProviderName;
            viewModel.Uri = provider.Uri;
            viewModel.Website = provider.Website;

            return viewModel;
        }
    }
}
