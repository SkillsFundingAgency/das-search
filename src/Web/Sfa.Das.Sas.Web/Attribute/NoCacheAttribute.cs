using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sfa.Das.Sas.Web.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // TODO: LWA Replicate caching functionality
            // filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            // filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            // filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            // filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            // filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
}