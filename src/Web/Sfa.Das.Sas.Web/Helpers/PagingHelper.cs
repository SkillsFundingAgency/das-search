using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices.Settings;

namespace Sfa.Das.Sas.Web.Helpers
{
    public class PagingHelper
    {
        public static int GetPageIndex(int actualPage)
        {
            var paginationSettings = DependencyResolver.Current.GetService<IPaginationSettings>();

            return (actualPage * paginationSettings.DefaultResultsAmount) - paginationSettings.DefaultResultsAmount;
        }
    }
}