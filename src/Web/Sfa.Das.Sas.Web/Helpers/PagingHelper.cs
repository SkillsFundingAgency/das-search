using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.Web.Helpers
{
    public class PagingHelper
    {
        private static IPaginationSettings _paginationSettings;

        public PagingHelper(IPaginationSettings paginationSettings)
        {
            _paginationSettings = paginationSettings;
        }

        public static int GetPageIndex(int actualPage)
        {
            return (actualPage * _paginationSettings.DefaultResultsAmount) - _paginationSettings.DefaultResultsAmount;
        }
    }
}