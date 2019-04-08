using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Resources;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class FatSearchResultsItemViewModel
    {
        public ICssClasses CssClasses { get; set; }

        public string Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string EquivalentText => EquivalenceLevelService.GetApprenticeshipLevel(Level.ToString());
        public int Duration { get; set; }
        public DateTime? LastDateForNewStarts { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
    }
}
