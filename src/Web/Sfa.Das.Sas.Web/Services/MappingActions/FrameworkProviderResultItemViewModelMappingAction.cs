using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    internal class FrameworkProviderResultItemViewModelMappingAction : IMappingAction<FrameworkProviderSearchResultsItem, FrameworkProviderResultItemViewModel>
    {
        public void Process(FrameworkProviderSearchResultsItem source, FrameworkProviderResultItemViewModel destination)
        {
            destination.EmployerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.EmployerSatisfaction);
            destination.LearnerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.LearnerSatisfaction);
        }
    }
}