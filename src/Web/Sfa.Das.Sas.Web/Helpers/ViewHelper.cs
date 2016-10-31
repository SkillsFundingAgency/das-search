namespace Sfa.Das.Sas.Web.Helpers
{
    public static class ViewHelper
    {
        public static string GetGaIndexAttrbute(int index)
        {
            if (index >= 1 && index <= 3)
            {
                return "1-3";
            }

            if (index >= 4 && index <= 6)
            {
                return "4-6";
            }

            if (index >= 7 && index <= 9)
            {
                return "7-9";
            }

            return "All Others";
        }
    }
}