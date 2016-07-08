namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Web.ViewModels;

    public class AggregationLevelValueResolver : ValueResolver<ApprenticeshipSearchResults, IEnumerable<LevelAggregationViewModel>>
    {
        protected override IEnumerable<LevelAggregationViewModel> ResolveCore(ApprenticeshipSearchResults source)
        {
            return CreateLevelAggregation(source.LevelAggregation, source.SelectedLevels?.ToList());
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