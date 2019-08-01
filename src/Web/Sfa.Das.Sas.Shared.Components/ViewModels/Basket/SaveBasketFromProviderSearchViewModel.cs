using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;

namespace Sfa.Das.Sas.Shared.Components.ViewModels.Basket
{
    public class SaveBasketFromProviderSearchViewModel
    {
        public int ItemId { get; set; }
        public TrainingProviderSearchViewModel SearchQuery { get; set; }
    }
}
