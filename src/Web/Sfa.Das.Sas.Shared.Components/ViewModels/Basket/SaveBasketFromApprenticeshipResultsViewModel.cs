using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;

namespace Sfa.Das.Sas.Shared.Components.ViewModels.Basket
{
    public class SaveBasketFromApprenticeshipResultsViewModel
    {
        public string ApprenticeshipId { get; set; }
        public SearchQueryViewModel SearchQuery { get; set; }
    }

    public class SaveBasketFromProviderDetailsViewModel
    {
        // LWA - Should we just use this class as model????
        public TrainingProviderDetailQueryViewModel SearchQuery { get; set; }
    }

    public class SaveBasketFromProviderSearchViewModel
    {
        public int Ukprn { get; set; }
        public TrainingProviderSearchViewModel SearchQuery { get; set; }
    }
}
