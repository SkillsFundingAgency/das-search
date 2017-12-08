using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;
using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    using System.Linq;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using ViewModels;

    public static class ProviderDetailViewModelMapper
    {
        public static ProviderDetailViewModel GetProviderDetailViewModel(Provider provider)
        {
            var viewModel = new ProviderDetailViewModel();
            if (provider.Aliases != null && provider.Aliases.Any())
            {
                viewModel.TradingNames = provider.Aliases.ToList().Aggregate((aggregatingTradingNames, aliasToAdd) => aggregatingTradingNames + ", " + aliasToAdd);
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

            if (provider.Ukprn == 10000055)
            {
                // MFCMFC Provider will change in CI-235
                viewModel.MarketingInfo =
                    @"Abingdon & Witney College offer the best performing apprenticeship programmes in Oxfordshire (Oxfordshire LMI, Winter 2016, p33), with 15/16 timely success rates at 79%, much higher than the national average. We have also recently invested over £30 million in our campuses, creating realistic working environments.\r\n\r\nOur apprenticeships are great employment preparation, combining work-based and classroom training, apprentices learn the ropes at college and build on this at work, receiving high levels of support throughout their programme.\r\n\r\nWe are currently working with a range of local employers to prepare for the Apprenticeship Levy, if you're not sure where to start then let us help!\r\n\r\nCall our team now on 01235 216 216 to book a free consultation";

                // MFCMFC Provider will emerge in CI-235
                viewModel.ApprenticeshipTrainingSummary =
                    new ApprenticeshipTrainingSummary
                    {
                        ApprenticeshipTrainingItems =
                            new List<ApprenticeshipTraining>
                            {
                                new ApprenticeshipTraining
                                {
                                    Name = "Accounting",
                                    Type = "Framework",
                                    TrainingType = ApprenticeshipTrainingType.Framework,
                                    Level = 2,
                                    Identifier = "454-3-1"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Electrical / electronic technical support engineer",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 6,
                                    Identifier = "10"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Embedded electronic systems design and development engineer",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 6,
                                    Identifier = "107"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Healthcare assistant practitioner",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 5,
                                    Identifier = "102"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Healthcare support worker",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 2,
                                    Identifier = "103"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Light Vehicle",
                                    Type = "Framework",
                                    TrainingType = ApprenticeshipTrainingType.Framework,
                                    Level = 2,
                                    Identifier = "436-3-1"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Light Vehicle",
                                    Type = "Framework",
                                    TrainingType = ApprenticeshipTrainingType.Framework,
                                    Level = 3,
                                    Identifier = "436-2-1"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Marketing",
                                    Type = "Framework",
                                    TrainingType = ApprenticeshipTrainingType.Framework,
                                    Level = 2,
                                    Identifier = "486-3-1"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Mechanical Manufacturing Engineering",
                                    Type = "Framework",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 3,
                                    Identifier = "539-2-3"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Network engineer",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 4,
                                    Identifier = "1"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Operations / departmental manager",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 5,
                                    Identifier = "104"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Papermaker",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 3,
                                    Identifier = "106"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Retailer",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 2,
                                    Identifier = "101"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Team leader / supervisor",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 3,
                                    Identifier = "105"
                                },
                                new ApprenticeshipTraining
                                {
                                    Name = "Transport planning technician",
                                    Type = "Standard",
                                    TrainingType = ApprenticeshipTrainingType.Standard,
                                    Level = 3,
                                    Identifier = "100"
                                }
                            },
                        Count = 15,
                        Ukprn = 10000055
                    };
            }

            return viewModel;
        }
    }
}