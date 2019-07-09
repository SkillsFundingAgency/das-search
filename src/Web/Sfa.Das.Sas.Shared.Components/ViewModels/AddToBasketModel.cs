namespace Sfa.Das.Sas.Shared.Components.ViewModels
{
    public class AddToBasketModel
    {
        public AddToBasketModel(ApprenticeshipDetailQueryViewModel viewModel)
        {
            ApprenticeshipId = viewModel.Id;
        }

        public string ApprenticeshipId { get; set; }
        public int? Ukprn { get; set; }
    }
}
