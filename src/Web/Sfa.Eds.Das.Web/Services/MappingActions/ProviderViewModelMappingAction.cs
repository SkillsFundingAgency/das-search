namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;
    using Sfa.Eds.Das.Web.ViewModels;

    public class ProviderViewModelMappingAction : IMappingAction<Provider, ProviderViewModel>
    {
        public void Process(Provider source, ProviderViewModel destination)
        {
            destination.EmployerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.EmployerSatisfaction);
            destination.LearnerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.LearnerSatisfaction);
        }
    }
}