using System;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Resources;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class FatSearchResultsItemViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string EquivalentText => EquivalenceLevelService.GetApprenticeshipLevel(Level.ToString());
        public int Duration { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
