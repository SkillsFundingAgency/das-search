namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;
    using Sfa.Eds.Das.Web.ViewModels;

    using Resources = Sfa.Eds.Das.Resources.EquivalenceLevels;

    internal class ApprenticeshipSearchResultViewModelMappingAction :
        IMappingAction<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>
    {
        public void Process(ApprenticeshipSearchResultsItem source, ApprenticeshipSearchResultItemViewModel destination)
        {
            destination.TypicalLengthMessage = StandardMappingHelper.GetTypicalLengthMessage(source.TypicalLength);
            destination.Level = GetLevelText(source.Level);
            destination.ApprenticeshipType = destination.StandardId != 0 ? "standard" : "framework";
        }

        private string GetLevelText(int item)
        {
            var equivalence = string.Empty;
            switch (item)
            {
                case 1:
                    equivalence = Resources.FirstLevel;
                    break;
                case 2:
                    equivalence = Resources.SecondLevel;
                    break;
                case 3:
                    equivalence = Resources.ThirdLevel;
                    break;
                case 4:
                    equivalence = Resources.FourthLevel;
                    break;
                case 5:
                    equivalence = Resources.FifthLevel;
                    break;
                case 6:
                    equivalence = Resources.SixthLevel;
                    break;
                case 7:
                    equivalence = Resources.SeventhLevel;
                    break;
                case 8:
                    equivalence = Resources.EighthLevel;
                    break;
                default:
                    equivalence = string.Empty;
                    break;
            }

            return !string.IsNullOrEmpty(equivalence) ? $"{item} (equivalent to {equivalence})" : string.Empty;
        }
    }
}