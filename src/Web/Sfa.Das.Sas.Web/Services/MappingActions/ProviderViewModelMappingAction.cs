using AutoMapper;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    public class ProviderViewModelMappingAction : IMappingAction<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>
    {
        public void Process(ApprenticeshipDetails source, ApprenticeshipDetailsViewModel destination)
        {
            destination.EmployerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.Product.EmployerSatisfaction);
            destination.LearnerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.Product.LearnerSatisfaction);
        }
    }
}