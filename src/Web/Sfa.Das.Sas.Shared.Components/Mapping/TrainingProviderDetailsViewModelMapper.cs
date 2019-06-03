using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class TrainingProviderDetailsViewModelMapper : ITrainingProviderDetailsViewModelMapper
    {
        readonly IFeedbackViewModelMapper _feedbackMapper;

        public TrainingProviderDetailsViewModelMapper(IFeedbackViewModelMapper feedbackMapper)
        {
            _feedbackMapper = feedbackMapper;
        }

        public TrainingProviderDetailsViewModel Map(ProviderDetailResponse source)
        {
            var employerSatisfationMessage =
              (source.Provider.EmployerSatisfaction > 0)
                  ? ProviderMappingHelper.GetPercentageText(source.Provider.EmployerSatisfaction)
                  : ProviderMappingHelper.GetPercentageText(null);

            var learnerSatisfationMessage =
                (source.Provider.LearnerSatisfaction > 0)
                    ? ProviderMappingHelper.GetPercentageText(source.Provider.LearnerSatisfaction)
                    : ProviderMappingHelper.GetPercentageText(null);

            var item = new TrainingProviderDetailsViewModel()
            {
                Email = source.Provider.Email,
                CurrentlyNotStartingNewApprentices = source.Provider.CurrentlyNotStartingNewApprentices,
                IsEmployerProvider = source.Provider.IsEmployerProvider,
                EmployerSatisfaction = source.Provider.EmployerSatisfaction,
                EmployerSatisfactionMessage = employerSatisfationMessage,
                IsHigherEducationInstitute = source.Provider.IsHigherEducationInstitute,
                LearnerSatisfaction = source.Provider.LearnerSatisfaction,
                LearnerSatisfactionMessage = learnerSatisfationMessage,
                NationalProvider = source.Provider.NationalProvider,
                Phone = source.Provider.Phone,
                UkPrn = source.Provider.Ukprn,
                ProviderName = source.Provider.ProviderName,
                Website = source.Provider.Website,
                MarketingInfo = source.Provider.MarketingInfo,
                HasParentCompanyGuarantee = source.Provider.HasParentCompanyGuarantee,
                IsNew = source.Provider.IsNew,
                IsLevyPayerOnly = source.Provider.IsLevyPayerOnly,

                ProviderFeedback = _feedbackMapper.Map(source.Provider.ProviderFeedback)

            };

            return item;
        }
    }
}
