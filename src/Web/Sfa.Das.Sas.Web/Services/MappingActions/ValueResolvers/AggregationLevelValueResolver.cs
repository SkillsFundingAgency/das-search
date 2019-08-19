using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    public class AggregationLevelValueResolver : IValueResolver<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel, IEnumerable<LevelAggregationViewModel>>
    {
        public IEnumerable<LevelAggregationViewModel> Resolve(ApprenticeshipSearchResponse source, ApprenticeshipSearchResultViewModel destination, IEnumerable<LevelAggregationViewModel> destMember, ResolutionContext context)
        {
            return CreateLevelAggregation(source.AggregationLevel, source.SelectedLevels?.ToList());
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