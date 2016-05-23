using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApprenticeshipSearchResultViewModelMappingAction : IMappingAction<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>
    {
        public void Process(ApprenticeshipSearchResults source, ApprenticeshipSearchResultViewModel destination)
        {
            destination.AggregationLevel = CreateLevelAggregation(source.LevelAggregation, source.SelectedLevels?.ToList());
            if (destination.ResultsToTake != 0)
            {
                destination.LastPage = (int)Math.Ceiling((double)source.TotalResults / source.ResultsToTake);
            }
        }

        private IEnumerable<LevelAggregationViewModel> CreateLevelAggregation(Dictionary<int, long?> levelAggregation, List<int> selectedList)
        {
            if (levelAggregation == null)
            {
                return new List<LevelAggregationViewModel>();
            }

            return levelAggregation.Select(
                item => new LevelAggregationViewModel
                { Value = item.Key.ToString(), Count = item.Value ?? 0L, Checked = selectedList != null && selectedList.Contains(item.Key) });
        }
    }
}