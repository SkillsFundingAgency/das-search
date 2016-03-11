namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;
    using Sfa.Eds.Das.Web.ViewModels;

    internal class ApprenticeshipSearchResultViewModelMappingAction :
        IMappingAction<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>
    {
        public void Process(ApprenticeshipSearchResultsItem source, ApprenticeshipSearchResultItemViewModel destination)
        {
            destination.TypicalLengthMessage = StandardMappingHelper.GetTypicalLengthMessage(source.TypicalLength);
        }
    }
}