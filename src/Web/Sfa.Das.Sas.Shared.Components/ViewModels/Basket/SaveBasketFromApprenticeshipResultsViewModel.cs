using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;

namespace Sfa.Das.Sas.Shared.Components.ViewModels.Basket
{
    public class SaveBasketFromApprenticeshipDetailsViewModel
    {
        public string ItemId { get; set; }
    }

    public class SaveBasketFromApprenticeshipResultsViewModel
    {
        public string ItemId { get; set; }
        public SearchQueryViewModel SearchQuery { get; set; }
    }

    public class SaveBasketFromProviderDetailsViewModel : TrainingProviderDetailQueryViewModel
    {
        public int ItemId { get; set; }
    }
    
    public class SaveBasketFromProviderSearchViewModel
    {
        public int ItemId { get; set; }
        public TrainingProviderSearchViewModel SearchQuery { get; set; }
    }
}
