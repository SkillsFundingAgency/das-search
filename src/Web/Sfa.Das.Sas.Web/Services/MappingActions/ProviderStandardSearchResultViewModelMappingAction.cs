namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    using System;

    using AutoMapper;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Web.ViewModels;

    internal class ProviderStandardSearchResultViewModelMappingAction :
        IMappingAction<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>
    {
        public void Process(ProviderStandardSearchResults source, ProviderStandardSearchResultViewModel destination)
        {
            destination.DeliveryModes = ProviderSearchMappingHelper.CreateDeliveryModes(source.TrainingOptionsAggregation, source.SelectedTrainingOptions);
            destination.LastPage = ProviderSearchMappingHelper.CalculateLastPage(source.TotalResults, source.ResultsToTake);
        }
    }
}