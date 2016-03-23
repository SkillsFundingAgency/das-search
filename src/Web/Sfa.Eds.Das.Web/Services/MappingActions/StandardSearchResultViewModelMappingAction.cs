namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;

    using ViewModels;

    public class StandardSearchResultViewModelMappingAction :
        IMappingAction<StandardSearchResultsItem, StandardSearchResultItemViewModel>
    {
        public void Process(StandardSearchResultsItem source, StandardSearchResultItemViewModel destination)
        {
            destination.TypicalLengthMessage = StandardMappingHelper.GetTypicalLengthMessage(source.TypicalLength);
        }
    }
}