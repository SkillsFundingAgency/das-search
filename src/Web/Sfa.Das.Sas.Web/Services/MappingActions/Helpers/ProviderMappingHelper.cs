using System.Collections.Generic;
using Sfa.Das.Sas.Web.Extensions;

namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    internal static class ProviderMappingHelper
    {
        internal static string GetPercentageText(double? level)
        {
            if (level == null)
            {
                return "no data available";
            }

            return $"{level}%";
        }

        internal static string GetDeliveryOptionText(List<string> deliveryOptions)
        {
            var deliveryOptionsMessage = string.Empty;
            if (deliveryOptions.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (deliveryOptions.Contains("DayRelease"))
            {
                deliveryOptionsMessage = "day release";
            }

            if (deliveryOptions.Contains("BlockRelease"))
            {
                deliveryOptionsMessage = deliveryOptionsMessage == string.Empty ? "block release" : string.Concat(deliveryOptionsMessage, ", block release");
            }

            if (deliveryOptions.Contains("100PercentEmployer"))
            {
                deliveryOptionsMessage = deliveryOptionsMessage == string.Empty ? "at your location" : string.Concat(deliveryOptionsMessage, ", at your location");
            }

            return $"{deliveryOptionsMessage}";
        }
    }
}