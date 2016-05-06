using System.Web.Mvc;

namespace Sfa.Das.Sas.Web
{
    using StackExchange.Profiling.Mvc;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ProfilingActionFilter());
        }
    }
}