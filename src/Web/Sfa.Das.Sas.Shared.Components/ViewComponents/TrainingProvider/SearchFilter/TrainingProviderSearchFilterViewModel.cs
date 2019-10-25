using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider
{
    public class TrainingProviderSearchFilterViewModel : TrainingProviderSearchViewModel
    {
        public TrainingProviderSearchFilterViewModel()
        {
            TrainingOptions = new List<FilterViewModel>();
            NationalProviders = new List<FilterViewModel>();
        }

        public IList<FilterViewModel> TrainingOptions { get; set; }

        public IList<FilterViewModel> NationalProviders { get; set; }
        public ProviderSearchResponseCodes Status { get; set; }
    }

    public class FilterViewModel
    {
        public string Value { get; set; }

        public long? Count { get; set; }

        public bool Checked { get; set; }
    }
}
