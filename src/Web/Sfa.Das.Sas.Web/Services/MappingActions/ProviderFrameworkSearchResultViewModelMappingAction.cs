namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Web.ViewModels;

    internal class ProviderFrameworkSearchResultViewModelMappingAction :
        IMappingAction<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>
    {
        public void Process(ProviderFrameworkSearchResults source, ProviderFrameworkSearchResultViewModel destination)
        {
            destination.DeliveryModes = ProviderSearchMappingHelper.CreateDeliveryModes(source.TrainingOptionsAggregation, source.SelectedTrainingOptions);
        }
    }
}