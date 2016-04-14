namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;
    using Sfa.Eds.Das.Web.ViewModels;

    internal class StandardProviderResultItemViewModelMappingAction : IMappingAction<StandardProviderSearchResultsItem, ProviderResultItemViewModel>
    {
        public void Process(StandardProviderSearchResultsItem source, ProviderResultItemViewModel destination)
        {
            destination.EmployerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.EmployerSatisfaction);
            destination.LearnerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.LearnerSatisfaction);
        }
    }
}