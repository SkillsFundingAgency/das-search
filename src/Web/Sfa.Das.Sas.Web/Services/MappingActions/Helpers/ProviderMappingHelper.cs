namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    internal static class ProviderMappingHelper
    {
        internal static string GetSatisfactionText(double? level)
        {
            if (level == null)
            {
                return "No data available";
            }

            return $"{level}%";
        }
    }
}