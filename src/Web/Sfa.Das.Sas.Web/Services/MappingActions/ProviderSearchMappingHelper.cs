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
                                       Checked = selectedTrainingOptions?.Contains(item.Key) ?? false,
                                       Value = item.Key
                                   });
            }

            var orderList = new List<string> { "dayrelease", "blockrelease", "100percentemployer" };
            return orderList.Select(i => viewModels.SingleOrDefault(m => m.Value == i)).WhereNotNull();
        }

        private static string GetName(string deliveryModeKey)
        {
            switch (deliveryModeKey)
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