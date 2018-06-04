namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Das.Sas.Resources;
    using Sfa.Das.Sas.Web.Extensions;
    using Sfa.Das.Sas.Web.ViewModels;

    public static class ProviderSearchMappingHelper
    {
        public static IEnumerable<DeliveryModeViewModel> CreateDeliveryModes(Dictionary<string, long?> trainingOptionsAggregation, IEnumerable<string> selectedTrainingOptions)
        {
            var viewModels = new List<DeliveryModeViewModel>();

            if (trainingOptionsAggregation.IsNullOrEmpty())
            {
                return viewModels;
            }

            foreach (var item in trainingOptionsAggregation)
            {
                viewModels.Add(new DeliveryModeViewModel
                                   {
                                       Title = GetName(item.Key),
                                       Count = item.Value ?? 0L,
                                       Checked = selectedTrainingOptions?.Contains(item.Key.ToLower()) ?? false,
                                       Value = item.Key
                                   });
            }

            var orderList = new List<string> { "dayrelease", "blockrelease", "100percentemployer" };
            return orderList.Select(i => viewModels.SingleOrDefault(m => m.Value.ToLower() == i)).WhereNotNull();
        }

        public static NationalProviderViewModel GetNationalProvidersAmount(Dictionary<string, long?> nationalProvidersAggregation, bool selectedNationalProvider)
        {
            var viewModel = new NationalProviderViewModel();

            if (nationalProvidersAggregation.IsNullOrEmpty())
            {
                return new NationalProviderViewModel();
            }

            foreach (var item in nationalProvidersAggregation)
            {
                if (item.Key == "1")
                {
                    viewModel = new NationalProviderViewModel
                    {
                        Title = "only national",
                        Count = item.Value ?? 0L,
                        Checked = selectedNationalProvider,
                        Value = item.Key
                    };
                }
            }

            return viewModel;
        }

        private static string GetName(string deliveryModeKey)
        {
            switch (deliveryModeKey.ToLower())
            {
                case "dayrelease":
                    return TrainingOptionService.GetApprenticeshipLevel("dayrelease");
                case "blockrelease":
                    return TrainingOptionService.GetApprenticeshipLevel("blockrelease");
                case "100percentemployer":
                    return TrainingOptionService.GetApprenticeshipLevel("100percentemployer");
                default:
                    return string.Empty;
            }
        }
    }
}